using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using App.Models;
using App.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace App.Services
{
    public class JwtService: IIdentityService
    {
        readonly IConfiguration _configuration;
        readonly IIdentityRepository _identityRepository;
        readonly SecurityKey _key;
        readonly string _securityAlgorithms = SecurityAlgorithms.HmacSha256;
        readonly ConcurrentDictionary<string, string> _sessionTokens = new ConcurrentDictionary<string, string>(); 

        public JwtService(
            IConfiguration configuration,
            IIdentityRepository identityRepository,
            SecurityKey key)
        {
            _configuration = configuration;    
            _identityRepository = identityRepository;
            _key = key;
        }

        public virtual object Authentificate(Identity identity)
        {
            if(_identityRepository.Find(identity.Login, identity.Password) == null)
                return null;

            return CreateToken(identity.Login, identity.Remember);
        }
       
        public virtual object Validate(ValidateToken validateToken)
        {
            var pricipal = new JwtSecurityTokenHandler().ValidateToken(validateToken.Token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false //don't care token's expiration
            }, out SecurityToken securityToken);

            var jwt = securityToken as JwtSecurityToken;
            var login = pricipal.Identity.Name;            
            if(!_sessionTokens.TryGetValue(login, out string refreshToken)
             || refreshToken != validateToken.Refresh
             || _securityAlgorithms != jwt?.Header?.Alg?.ToUpper())
                return null;

            /*if(!_sessionTokens.TryGetValue(validateToken.Login, out string refreshToken) 
                || refreshToken != validateToken.Refresh)
                return null;*/

            return CreateToken(validateToken.Login, true);
        }

        protected virtual string CreateRefresh(string login)
        {
            var refreshrnd = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(refreshrnd);
            var token = Convert.ToBase64String(refreshrnd);
            _sessionTokens.AddOrUpdate(login, token, (login, old) => token);
            return token;
        }

        protected virtual object CreateToken(string login, bool remember)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["TokenKey"]);
            var expires = DateTime.UtcNow.AddMinutes(20);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(_key, _securityAlgorithms),
                Audience = _configuration["Client"],
                Issuer = _configuration["JwtServer"]
            };
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            var refresh = remember ? CreateRefresh(login) : string.Empty;

            return new 
            {
                token = token,
                userName = login,
                expiresToken = (expires - DateTime.UtcNow).TotalMinutes,
                refresh = refresh
            };
        }
    }
}
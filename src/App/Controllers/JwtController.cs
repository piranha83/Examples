using App.Filters;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{    
    [Authorize]
    [ApiController]
    [ValidateModel]
    [Route("api/[controller]")]    
    public class JwtController: ControllerBase
    {
        readonly IIdentityService _identityService;

        public JwtController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        [Route(nameof(Authenticate))]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] Identity identity)
        {
            var data = _identityService.Authentificate(identity);
            return new JsonResult(data);
        }

        /*[Route(nameof(Refresh))]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Refresh([FromBody] ValidateToken validateToken)
        {                 
            var data = _identityService.Validate(validateToken);
            return new JsonResult(data);
        }*/
    }
}
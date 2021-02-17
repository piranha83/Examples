using App.Models;

namespace App.Services
{
    public interface IIdentityService
    {
        object Authentificate(Identity identity);
        object Validate(ValidateToken validateToken);
    }
}
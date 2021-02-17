using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.Repositories
{
    public interface IIdentityRepository
    {
        Identity Find(string login, string password);
    }
}

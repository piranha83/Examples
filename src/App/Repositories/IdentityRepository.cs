using System.Collections.Generic;
using System.Linq;
using App.Models;

namespace App.Repositories
{
    public class IdentityRepository: IIdentityRepository
    {
        private readonly List<Identity> _identity = new List<Identity>
        {
            new Identity { Login="admin", Password="E5CY*.HbTn'8}[*z" },
            new Identity { Login="demo", Password="hN4u_R(U" }
        };

        public virtual Identity Find(string login, string password)
        {           
            return _identity.FirstOrDefault(m=> 
                m.Login.ToUpper().Contains(login.ToUpper()) 
                    && m.Password.ToUpper().Contains(password.ToUpper())); 
        }
    }
}

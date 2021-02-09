using System.Threading.Tasks;

namespace App.Repositories
{
    public interface ISession
    {        
        Task Save();
    }
}
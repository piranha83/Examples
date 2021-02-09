using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class EfSession<TContext>: ISession
    where TContext: DbContext
    {        
        private readonly TContext _dbContext;

        public EfSession(TContext dbContext) 
            => _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task Save() 
            => await _dbContext.SaveChangesAsync();
    }
}
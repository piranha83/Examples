using System;
using System.Linq;
using App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace App.Extensions
{
    public static class RepositoryExtensions
    {   
        public static IQueryable<TEntity> AsQueryable<TEntity>(this IRepository<TEntity> repository)
            where TEntity : class, IEntity<int> 
        {
            if(repository is EfRepository<TEntity> efRepository)
                return efRepository.AsQueryable().AsNoTracking();

            throw new NotImplementedException();
        }
    }
}
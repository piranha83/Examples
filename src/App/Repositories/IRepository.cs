using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Repositories
{
    public interface IRepository<TEntity, TKey>
    where TEntity: class
    {        
        Task<TEntity> Find(TKey id);
        Task<IList<TEntity>> Find();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TKey id);
    }

    public interface IRepository<TEntity>: IRepository<TEntity, int>
    where TEntity: class
    {        
    }
}
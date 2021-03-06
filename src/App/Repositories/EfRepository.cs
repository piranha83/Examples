using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Extensions;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class EfRepository<TEntity, TKey, TContext>// : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TContext: DbContext
    {
        private readonly TContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public EfRepository(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = dbContext.Set<TEntity>() ?? throw new ArgumentNullException(nameof(_dbSet));
        }

        public virtual async Task<TEntity> Find(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IList<TEntity>> Find()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual void Add(TEntity entity)
        {
            var st = _dbContext.Entry(entity).State;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
           _dbSet.Add(entity);
           _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public virtual void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TKey id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            _dbSet.Remove(entityToDelete);
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable<TEntity>();
        }

        public virtual IQueryable<TEntity> IncludeAll(int maxDepth = int.MaxValue)
        {
            var result = this.AsQueryable();
            var includePaths = _dbContext.GetNavigation<TEntity, TContext>(maxDepth);

            foreach (var includePath in includePaths)
            {
                result = result.Include(includePath);
            }            
            return result.AsNoTracking();
        }
    }

    public class EfRepository<TEntity> : EfRepository<TEntity, int, ApplicationDbContext>, IRepository<TEntity>
    where TEntity : class, IEntity<int>
    {
        public EfRepository(ApplicationDbContext dbContext):base(dbContext)
        {            
        }
    }
}

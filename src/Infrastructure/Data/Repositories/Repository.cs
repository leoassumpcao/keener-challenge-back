using API.Core.Interfaces.Repositories;
using API.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _db;
        protected readonly DbSet<TEntity> _dbset;

        public Repository(AppDbContext context)
        {
            _db = context;
            _dbset = _db.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
            => _dbset.AsNoTracking();

        public async Task<TType?> Get<TType>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TType>> select) where TType : class
        {
            return await _dbset.AsNoTracking().Where(where).Select(select).FirstOrDefaultAsync();
        }

        public async Task<ICollection<TType>> Find<TType>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TType>> select) where TType : class
        {
            return await _dbset.AsNoTracking().Where(where).Select(select).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _dbset.AsNoTracking().ToListAsync();

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
            => await _dbset.FindAsync(id);

        public void Add(TEntity entity)
            => _dbset.Add(entity);

        public void AddRange(IEnumerable<TEntity> entitys)
            => _dbset.AddRange(entitys);

        public void Update(TEntity entity)
            => _dbset.Update(entity);

        public void Remove(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbset.RemoveRange(entities);
        }

        public async Task Remove(Guid id)
        {
            TEntity? entity = await GetByIdAsync(id);
            if (entity is not null)
                _dbset.Remove(entity);
        }

        public async Task<int> SaveChanges()
            => await _db.SaveChangesAsync();

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

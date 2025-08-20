using Challenge.Core.Abstraction;
using Core.Abstraction;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Challenge.Infra.Repositories; // ajustá el namespace a tu carpeta Data real


namespace Challenge.Infra.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        public EfRepository(AppDbContext db) => _db = db;

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _db.Set<T>().FindAsync(new object?[] { id }, ct);

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T>? spec = null, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.GetQuery(_db.Set<T>().AsQueryable(), spec);
            return await query.ToListAsync(ct);
        }

        public async Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            await _db.Set<T>().AddAsync(entity, ct);
            return entity;
        }

        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _db.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            _db.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}
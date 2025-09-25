using Domain.Entities;
using Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MinimalAPIs.IRepository;
using System.Linq.Expressions;

namespace MinimalAPIs.Repository;

public class GenericRepository<T>(AppDbContext _context) : IGenericRepository<T> where T : BaseEntity
{
    protected DbSet<T> _dbSet = _context.Set<T>();

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        await SaveChangesAsync(ct);
        return entity;
    }

    public async Task<int> DeleteByIdAsync(int id, CancellationToken ct = default)
        => await _dbSet.Where(p => p.Id == id).ExecuteDeleteAsync(ct);

    public async Task<bool> Exists(Expression<Func<T, bool>> filter, CancellationToken ct = default)
        => await _dbSet.AsNoTracking().AnyAsync(filter, ct);

    public async Task<T?> GetByIdAsync(int id, bool asNoTracking = true, CancellationToken ct = default)
    {
        var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
        return await query.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public IQueryable<T> Query(bool asNoTracking = true)
        => asNoTracking ? _dbSet.AsNoTracking() : _dbSet;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task<int> UpdateAsync(Expression<Func<T, bool>> filter, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setter, CancellationToken ct = default)
        => await _dbSet.Where(filter).ExecuteUpdateAsync(setter, ct);
}

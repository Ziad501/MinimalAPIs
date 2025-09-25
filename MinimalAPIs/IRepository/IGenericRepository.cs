using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MinimalAPIs.IRepository;

public interface IGenericRepository<T> where T : BaseEntity
{
    IQueryable<T> Query(bool asNoTracking = true);
    Task<T?> GetByIdAsync(int id, bool asNoTracking = true, CancellationToken ct = default);
    Task<bool> Exists(Expression<Func<T, bool>> filter, CancellationToken ct = default);
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task<int> UpdateAsync(Expression<Func<T, bool>> filter, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setter, CancellationToken ct = default);
    Task<int> DeleteByIdAsync(int id, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

using System.Linq.Expressions;
using VeterinaryClinic.Entities;

namespace VeterinaryClinic.DataAccess.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T entity);

    void Update(T entity);
    void Delete(T entity);
}

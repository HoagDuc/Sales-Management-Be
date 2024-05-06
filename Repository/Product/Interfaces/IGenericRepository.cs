using System.Linq.Expressions;

namespace ptdn_net.Repository.interfaces;

public interface IGenericRepository<T, in TK> where T : class where TK : IComparable<TK>
{
    Task<T?> GetByIdAsync(TK id);

    Task<IEnumerable<T>> GetAll();

    Task InsertAsync(T entity);

    void Update(T entity);

    Task<T?> GetByCondition(Expression<Func<T, bool>> expression);

    Task<IEnumerable<T>?> GetAllByCondition(Expression<Func<T, bool>> expression);
    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);

    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

    Task<int> CountByIdAsync(TK id);

    Task<int> CountAsync(Expression<Func<T, bool>> expression);
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class GenericRepository<T, TK> : IGenericRepository<T, TK> where T : class where TK : IComparable<TK>
{
    public readonly DbSet<T> DbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        DbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(TK id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await DbSet.ToListAsync();
    }

    public async Task InsertAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public async Task<T?> GetByCondition(Expression<Func<T, bool>> expression)
    {
        return await DbSet.Where(expression).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>?> GetAllByCondition(Expression<Func<T, bool>> expression)
    {
        return await DbSet.Where(expression).ToListAsync();
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await DbSet.AnyAsync(expression);
    }

    public async Task<int> CountByIdAsync(TK id)
    {
        return await DbSet.CountAsync(e => e.Equals(id));
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        return await DbSet.CountAsync(expression);
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }
}
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Wondvoy.Application.Abstractions.Repositories.Generic;
using Wondvoy.Domain.Entities.Common;
using Wondvoy.Persistence.Contexts;

namespace Wondvoy.Persistence.Implementations.Repositories.Generic;

public class Repository<T> : IRepository<T> where T : BaseEntity
{

    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }



    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public IQueryable<T> GetAll(bool ignoreFilter = false, params string[] includes)
    {
        var query = _context.Set<T>().AsQueryable();

        if (ignoreFilter)
            query = query.IgnoreQueryFilters();

        query = _addIncludes(includes, query);

        return query;
    }

    public IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression, bool ignoreFilter = false, params string[] includes)
    {
        var query = _context.Set<T>().Where(expression).AsQueryable();

        if (ignoreFilter)
            query = query.IgnoreQueryFilters();

        query = _addIncludes(includes, query);

        return query;
    }

    public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression, bool ignoreFilter = false, params string[] includes)
    {
        var query = _context.Set<T>().AsQueryable();

        if (ignoreFilter)
            query = query.IgnoreQueryFilters();

        query = _addIncludes(includes, query);

        var entity = await query.FirstOrDefaultAsync(expression);

        return entity;
    }

    public void HardDelete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> expression, bool ignoreFilter = false)
    {
        var query = _context.Set<T>().AsQueryable();

        if (ignoreFilter)
            query = query.IgnoreQueryFilters();

        var result = await query.AnyAsync(expression);

        return result;
    }

    public IQueryable<T> OrderBy(IQueryable<T> query, Expression<Func<T, object>> expression)
    {
        return query.OrderBy(expression);
    }

    public IQueryable<T> OrderByDescending(IQueryable<T> query, Expression<Func<T, object>> expression)
    {
        return query.OrderByDescending(expression);
    }

    public IQueryable<T> Paginate(IQueryable<T> query, int limit, int page = 1)
    {
        var result = query.Skip((page - 1) * limit).Take(limit);

        return result;
    }

    public void Repair(T entity)
    {
        if (entity is BaseAuditableEntity baseAuditableEntity)
        {
            baseAuditableEntity.IsDeleted = false;
        }
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void SoftDelete(T entity)
    {
        if (entity is BaseAuditableEntity baseAuditableEntity)
        {
            baseAuditableEntity.IsDeleted = true;
        }
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }



    private static IQueryable<T> _addIncludes(string[] includes, IQueryable<T> query)
    {
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query;
    }

}

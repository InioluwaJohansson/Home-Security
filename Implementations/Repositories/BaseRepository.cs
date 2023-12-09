using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using Home_Security.Context;
using Home_Security.Interfaces.Repositories;
using Home_Security.Contracts;
using Microsoft.EntityFrameworkCore;
namespace Home_Security.Implementations.Repositories;
public class BaseRepository<T> : IRepo<T> where T : class, new()
{
    protected HomeSecurityContext context;
    public async Task<T> Create(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> Update(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<T> Get(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().FirstOrDefaultAsync(expression);
    }
    public async Task<IList<T>> GetAll()
    {
        return await context.Set<T>().ToListAsync();
    }
    public async Task<bool> Delete(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<IList<T>> GetByExpression(Expression<Func<T, bool>> expression)
    {
        return await context.Set<T>().Where(expression).ToListAsync();
    }
}

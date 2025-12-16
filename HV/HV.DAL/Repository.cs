using System.Collections;
using System.Linq.Expressions;
using HV.DAL.Abstractions;
using HV.DAL.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HV.DAL;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _table = context.Set<TEntity>();

    public Type ElementType => ((IQueryable<TEntity>)_table).ElementType;
    public Expression Expression => ((IQueryable<TEntity>)_table).Expression;
    public IQueryProvider Provider => ((IQueryable<TEntity>)_table).Provider;

    public IEnumerator<TEntity> GetEnumerator() => ((IEnumerable<TEntity>)_table).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public async Task InsertAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _table.AddAsync(entity);
    }

    public async Task InsertManyAsync(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await _table.AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _table.Update(entity);
    }

    public void UpdateMany(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        _table.UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _table.Remove(entity);
    }

    public void DeleteMany(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        _table.RemoveRange(entities);
    }

    public void SoftDelete<TSoftDeletable>(TSoftDeletable entity)
        where TSoftDeletable : SoftDeletableEntity
    {
        ArgumentNullException.ThrowIfNull(entity);

        entity.IsDeleted = true;
        _table.Update((entity as TEntity)!);
    }

    public void SoftDeleteMany<TSoftDeletable>(IEnumerable<TSoftDeletable> entities)
        where TSoftDeletable : SoftDeletableEntity
    {
        ArgumentNullException.ThrowIfNull(entities);
        foreach (var entity in entities)
        {
            entity.IsDeleted = true;
            _table.Update((entity as TEntity)!);
        }
    }
}
using HV.DAL.Entities.Abstractions;

namespace HV.DAL.Abstractions;

public interface IRepository<TEntity> : IQueryable<TEntity>
    where TEntity : BaseEntity
{
    Task InsertAsync(TEntity entity);
    Task InsertManyAsync(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    void UpdateMany(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);
    void DeleteMany(IEnumerable<TEntity> entities);
    
    void SoftDelete<TSoftDeletable>(TSoftDeletable entity) where TSoftDeletable : SoftDeletableEntity;
    void SoftDeleteMany<TSoftDeletable>(IEnumerable<TSoftDeletable> entities)  where TSoftDeletable : SoftDeletableEntity;
}
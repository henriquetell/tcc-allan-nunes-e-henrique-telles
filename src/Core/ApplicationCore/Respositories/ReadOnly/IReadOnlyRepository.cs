using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Respositories
{
    public interface IReadOnlyRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
    {
        TEntity Recuperar(TKey id);

        List<TEntity> Listar();
    }

    public interface IReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity, int>
        where TEntity : EntityBase
    { }
}

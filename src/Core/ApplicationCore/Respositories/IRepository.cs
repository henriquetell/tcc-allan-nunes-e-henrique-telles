using ApplicationCore.Entities;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Respositories
{
    public interface IRepository
    {
    }

    public interface IRepository<TEntity, TKey> : IRepository where TEntity : EntityBase<TKey>
    {
        void Salvar(TEntity entity, bool? isNew = null);
        void Salvar(IEnumerable<TEntity> entity, Func<TEntity, bool> isNew);
        void Excluir(TEntity entity);
        void Excluir(TKey id);
    }

    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : EntityBase
    {

    }
}

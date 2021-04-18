using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using Framework.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal abstract class RespositoryBase
    {
        protected readonly EfContext DbContext;

        protected RespositoryBase(EfContext dbContext)
        {
            DbContext = dbContext;
        }

        protected static List<TElement> Paginar<TElement>(IQueryable<TElement> query, PaginadorInfo paginador)
        {
            paginador.SetTotalRegistro(query.LongCount());

            int totalExibidos;

            try
            {
                totalExibidos = paginador.TotalPaginas < paginador.TotalExibidos / paginador.TamanhoPagina
                    ? 0
                    : paginador.TotalExibidos;
            }
            catch (DivideByZeroException)
            {
                totalExibidos = 0;
            }

            return query.Skip(totalExibidos).Take(paginador.TamanhoPagina).ToList();
        }
    }

    internal abstract class RespositoryBase<TEntity, TKey> : RespositoryBase, IReadOnlyRepository<TEntity, TKey>, IRepository<TEntity, TKey>
        where TEntity : EntityBase<TKey>
    {
        protected RespositoryBase(EfContext dbContext) : base(dbContext) { }

        public virtual TEntity Recuperar(TKey id) => DbContext.Find<TEntity>(id);

        public virtual void Salvar(TEntity entity, bool? isNew)
        {
            var entry = DbContext.Entry(entity);

            if (!isNew.HasValue)
                isNew = entry.State == EntityState.Detached;

            if (isNew.Value)
                DbContext.Set<TEntity>().Add(entity);
            else
                entry.State = EntityState.Modified;

            DbContext.SaveChanges();
        }

        public virtual void Salvar(IEnumerable<TEntity> entity, Func<TEntity, bool> isNew)
        {
            if (entity == null || !entity.Any())
                return;

            var dbSet = DbContext.Set<TEntity>();

            foreach (var item in entity)
            {
                if (isNew != null && isNew.Invoke(item))
                    dbSet.Add(item);
                else
                {
                    var entry = DbContext.Entry(item);
                    entry.State = EntityState.Modified;
                }
            }

            DbContext.SaveChanges();
        }

        public virtual void Excluir(TKey id)
        {
            var entity = DbContext.Set<TEntity>().Find(id);
            DbContext.Set<TEntity>().Remove(entity);
            DbContext.SaveChanges();
        }

        public virtual void Excluir(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            DbContext.SaveChanges();
        }

        public List<TEntity> Listar() => DbContext.Set<TEntity>().ToList();
    }

    internal abstract class RespositoryBase<TEntity> : RespositoryBase<TEntity, int>
        where TEntity : EntityBase
    {
        protected RespositoryBase(EfContext dbContext)
            : base(dbContext)
        {
        }
    }
}

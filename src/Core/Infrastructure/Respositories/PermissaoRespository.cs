using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class PermissaoRespository : RespositoryBase<PermissaoEntity, Guid>, IPermissaoRepository
    {
        public PermissaoRespository(EfContext dbContext)
            : base(dbContext)
        { }

        public void Sincronizar(IEnumerable<PermissaoEntity> permissoes)
        {
            var idsPermissoes = permissoes.Select(nf => nf.Id).ToArray();

            var remover = DbContext.Permissao
                .Where(f => !idsPermissoes.Contains(f.Id));

            var novas = permissoes
                .Where(f => !DbContext.Permissao.Any(nf => nf.Id == f.Id));

            DbContext.RemoveRange(remover);
            DbContext.AddRange(novas);
            DbContext.SaveChanges();
        }
    }
}

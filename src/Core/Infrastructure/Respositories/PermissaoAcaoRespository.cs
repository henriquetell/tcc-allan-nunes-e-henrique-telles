using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class PermissaoAcaoRespository : RespositoryBase<PermissaoAcaoEntity, Guid>, IPermissaoAcaoRepository
    {
        public PermissaoAcaoRespository(EfContext dbContext)
            : base(dbContext)
        { }

        public void Sincronizar(IEnumerable<PermissaoAcaoEntity> permissoesAcoes)
        {
            var idsAdminFuncionalidade = permissoesAcoes.Select(nfa => nfa.Id).ToArray();

            var remover = DbContext.PermissaoAcao.Where(fa => !idsAdminFuncionalidade.Contains(fa.Id)).ToList();

            var novas = permissoesAcoes
                .Where(fa => !DbContext.PermissaoAcao.Any(nfa => nfa.IdPermissao == fa.IdPermissao && nfa.Id == fa.Id));

            var idsPermissoesAcoes = remover.Select(y => y.Id);

            var acoesUsuarios =
                DbContext.GrupoUsuarioPermisaoAcao.Where(c => idsPermissoesAcoes.Contains(c.IdPermissaoAcao)).ToList();

            DbContext.GrupoUsuarioPermisaoAcao.RemoveRange(acoesUsuarios);
            DbContext.RemoveRange(remover);

            DbContext.AddRange(novas);
            DbContext.SaveChanges();
        }
    }
}

using ApplicationCore.Entities;
using ApplicationCore.Respositories;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Respositories
{
    internal class GrupoUsuarioPermisaoAcaoRepository : RespositoryBase<GrupoUsuarioPermisaoAcaoEntity>, IGrupoUsuarioPermisaoAcaoRepository
    {
        public GrupoUsuarioPermisaoAcaoRepository(EfContext dbContext)
            : base(dbContext)
        { }

        public void ExcluirTodos(int id)
        {
            var itens = DbContext.GrupoUsuarioPermisaoAcao.Where(c => c.IdGrupoUsuario == id);
            DbContext.GrupoUsuarioPermisaoAcao.RemoveRange(itens);
            DbContext.SaveChanges();
        }

        public void SalvarTodos(IEnumerable<GrupoUsuarioPermisaoAcaoEntity> permissoes)
        {
            DbContext.GrupoUsuarioPermisaoAcao.AddRange(permissoes);
            DbContext.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IGrupoUsuarioPermisaoAcaoRepository : IGrupoUsuarioPermisaoAcaoReadOnlyRepository, IRepository<GrupoUsuarioPermisaoAcaoEntity>
    {
        void ExcluirTodos(int id);
        void SalvarTodos(IEnumerable<GrupoUsuarioPermisaoAcaoEntity> permissoes);
    }
}

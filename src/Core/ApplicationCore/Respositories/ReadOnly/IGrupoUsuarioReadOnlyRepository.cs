using ApplicationCore.Entities;
using Framework.Data;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IGrupoUsuarioReadOnlyRepository : IReadOnlyRepository<GrupoUsuarioEntity>
    {
        GrupoUsuarioEntity RecuperarPorNome(string nomeGrupo);
        List<GrupoUsuarioEntity> Listar(string consulta, PaginadorInfo paginador);
        List<Guid> ListarPermissaoAcaoPorUsuario(int idUsuario);
    }
}

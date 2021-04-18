using ApplicationCore.Entities;
using Framework.Data;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IUsuarioReadOnlyRepository : IReadOnlyRepository<UsuarioEntity>
    {
        UsuarioEntity RecuperarPorEmail(string email);
        List<UsuarioEntity> Listar(string consulta, PaginadorInfo paginador);
    }
}

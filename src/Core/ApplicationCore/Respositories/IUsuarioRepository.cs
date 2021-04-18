using System;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IUsuarioRepository : IUsuarioReadOnlyRepository, IRepository<UsuarioEntity>
    {
    }
}

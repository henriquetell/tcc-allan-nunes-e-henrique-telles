using System;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IGrupoUsuarioRepository : IGrupoUsuarioReadOnlyRepository, IRepository<GrupoUsuarioEntity>
    {
    }
}

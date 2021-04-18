using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IPermissaoRepository : IPermissaoReadOnlyRepository, IRepository<PermissaoEntity, Guid>
    {
        void Sincronizar(IEnumerable<PermissaoEntity> permissoes);
    }
}

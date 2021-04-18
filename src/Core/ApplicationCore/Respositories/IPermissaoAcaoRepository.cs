using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IPermissaoAcaoRepository : IPermissaoAcaoReadOnlyRepository, IRepository<PermissaoAcaoEntity, Guid>
    {
        void Sincronizar(IEnumerable<PermissaoAcaoEntity> permissoesAcoes);
    }
}

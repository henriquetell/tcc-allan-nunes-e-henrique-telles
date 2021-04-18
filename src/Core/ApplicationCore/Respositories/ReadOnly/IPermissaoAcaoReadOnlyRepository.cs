using ApplicationCore.Entities;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IPermissaoAcaoReadOnlyRepository : IReadOnlyRepository<PermissaoAcaoEntity, Guid>
    {
    }
}

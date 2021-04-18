using ApplicationCore.Entities;
using System;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IPermissaoReadOnlyRepository : IReadOnlyRepository<PermissaoEntity, Guid>
    {
    }
}

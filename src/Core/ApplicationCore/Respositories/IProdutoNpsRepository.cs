using System;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IProdutoNpsRepository : IProdutoNpsReadOnlyRepository, IRepository<ProdutoNpsEntity, Guid>
    {
    }
}

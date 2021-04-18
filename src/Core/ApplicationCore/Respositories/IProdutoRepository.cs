using System.Collections.Generic;
using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IProdutoRepository : IProdutoReadOnlyRepository, IRepository<ProdutoEntity>
    {
    }
}

using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IProdutoNpsReadOnlyRepository : IReadOnlyRepository<ProdutoNpsEntity, Guid>
    {
        List<ProdutoNpsDataValue> ListarPorProduto(int idProduto);
    }
}

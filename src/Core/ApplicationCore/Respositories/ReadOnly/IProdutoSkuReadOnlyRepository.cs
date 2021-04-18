using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Respositories.ReadOnly
{
    public interface IProdutoSkuReadOnlyRepository : IReadOnlyRepository<ProdutoSkuEntity>
    {
        ProdutoSkuDataValue Recuperar(int idProduto, int idProdutoSku);
        List<ProdutoSkuDataValue> ListarParaFiltro(int idProduto);

        List<ProdutoSkuDataValue> ListarComEstoque(
            int idProduto,
            bool somenteComEstoque,
            params int[] idProdutoSku);
        List<ProdutoSkuDataValue> Listar(int id);
    }
}

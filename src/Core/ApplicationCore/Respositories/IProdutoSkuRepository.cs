using ApplicationCore.DataValue;
using ApplicationCore.Entities;
using ApplicationCore.Respositories.ReadOnly;

namespace ApplicationCore.Respositories
{
    public interface IProdutoSkuRepository : IProdutoSkuReadOnlyRepository, IRepository<ProdutoSkuEntity>
    {
        AdicionarProdutoSkuDataValue RecuperarParaAdicionar(int idProdutoSku);
    }
}

using ApplicationCore.DataValue;
using ApplicationCore.Enuns;

namespace Shop.ViewModels.Produto
{
    public class ProdutoSkuViewModel
    {
        public ProdutoSkuViewModel()
        { }

        public ProdutoSkuViewModel(ProdutoSkuDataValue sku)
        {
            Id = sku.Id;
            IdProduto = sku.IdProduto;
            TipoSku = sku.TipoSku;
            Descricao = sku.Descricao;
            Saldo = sku.Saldo;
        }

        public int Id { get; set; }
        public int IdProduto { get; set; }
        public ETipoSku TipoSku { get; set; }
        public string Descricao { get; set; }
        public int Saldo { get; set; }
    }
}

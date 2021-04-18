using ApplicationCore.DataValue;

namespace Shop.ViewModels.Pedido
{
    public class PedidoItemDetalhesViewModel
    {
        public PedidoItemDetalhesViewModel() { }

        public PedidoItemDetalhesViewModel(PedidoItemDataValue c)
        {
            Id = c.Id;
            IdPedido = c.IdPedido;
            Quantidade = c.Quantidade;
            Valor = c.Valor;
            ValorTaxa = c.ValorTaxa;
            ValorFrete = c.ValorFrete;
            IdProduto = c.IdProduto;
            Produto = c.Produto;
            Sku = c.Sku;
            Imagem = c.Imagem;
        }

        public int Id { get; set; }
        public int IdPedido { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Produto { get; set; }
        public string Sku { get; set; }
        public string Imagem { get; set; }
        public int IdProduto { get; set; }
        public decimal? ValorTaxa { get; set; }
        public decimal? ValorFrete { get; set; }
    }
}

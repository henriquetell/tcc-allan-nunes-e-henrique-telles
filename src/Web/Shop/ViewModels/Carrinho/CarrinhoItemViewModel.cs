using ApplicationCore.DataValue;

namespace Shop.ViewModels.Carrinho
{
    public class CarrinhoItemViewModel
    {
        public CarrinhoItemViewModel()
        { }

        public CarrinhoItemViewModel(CarrinhoItemDataValue item)
        {
            Id = item.Id;
            IdCarrinho = item.IdCarrinho;
            IdProdutoSku = item.IdProdutoSku;
            Quantidade = item.Quantidade;
            Descricao = item.Descricao;
            PrecoDe = item.PrecoDe;
            PrecoPor = item.PrecoPor;
            Titulo = item.Titulo;
            Imagem = item.Imagem;
        }

        public int Id { get; set; }
        public int IdCarrinho { get; set; }
        public int IdProdutoSku { get; set; }
        public int Quantidade { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoDe { get; set; }
        public decimal PrecoPor { get; set; }
        public string Titulo { get; set; }
        public string Imagem { get; set; }
    }
}

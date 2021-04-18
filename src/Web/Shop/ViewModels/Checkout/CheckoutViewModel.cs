using System.Collections.Generic;
using Shop.ViewModels.Carrinho;

namespace Shop.ViewModels.Checkout
{
    public class CheckoutViewModel
    {
        public int? IdCarrinho { get; set; }
        public decimal? ValorTotal { get; set; }
        public int QuantidadeMaximaParcelamento { get; set; }
        public PagamentoViewModel Pagamento { get; set; } = new PagamentoViewModel();
        public IEnumerable<CarrinhoItemViewModel> Itens { get; set; } = new List<CarrinhoItemViewModel>();
    }
}

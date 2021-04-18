using System.Collections.Generic;

namespace Shop.ViewModels.Carrinho
{
    public class CarrinhoViewModel
    {
        public int IdCarrinho { get; set; }
        public List<CarrinhoItemViewModel> Itens { get; set; } = new List<CarrinhoItemViewModel>();
    }
}

using System;

namespace Admin.ViewModels.Produto
{
    public class ProdutoNpsViewModel
    {
        public ProdutoNpsViewModel()
        {

        }

        public ProdutoNpsViewModel(int? idProduto)
        {
            IdProduto = idProduto;
        }

        public string Email { get; set; }
        public int? IdProduto { get; set; }
        public DateTime? DataLimite { get; set; }
    }
}

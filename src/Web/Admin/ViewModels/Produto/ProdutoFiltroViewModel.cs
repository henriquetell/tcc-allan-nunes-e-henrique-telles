using Framework.Data;
using System.Collections.Generic;

namespace Admin.ViewModels.Produto
{
    public class ProdutoFiltroViewModel
    {
        public string Consulta { get; set; }
        public PaginadorInfo Paginador { get; set; }

        public List<ProdutoViewModel> Itens { get; set; } = new List<ProdutoViewModel>();
    }
}

using Admin.Filters;
using Admin.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class SharedController : BaseController
    {
        private ConteudoServiceWeb ConteudoServiceWeb => GetService<ConteudoServiceWeb>();
        private ProdutoServiceWeb ProdutoServiceWeb => GetService<ProdutoServiceWeb>();

        [HttpPost]
        [AuthUsuarioFilter]
        public IActionResult ListarSkus(int idProduto) => Json(ProdutoServiceWeb.ListarSkus(idProduto));
    }
}

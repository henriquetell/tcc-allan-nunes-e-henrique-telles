using ApplicationCore.Interfaces.Logging;
using ApplicationCore.Respositories.ReadOnly;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.ViewModels;
using Shop.ViewModels.Produto;

namespace Shop.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProdutoController : BaseController
    {
        private readonly IAppLogger<ProdutoController> _logger;
        private readonly IProdutoReadOnlyRepository _produtoReadOnly;
        private readonly IProdutoImagemReadOnlyRepository _produtoImagemReadOnly;
        private readonly IProdutoSkuReadOnlyRepository _produtoSkuReadOnly;

        public ProdutoController(
            IAppLogger<ProdutoController> logger,
            IProdutoImagemReadOnlyRepository produtoImagemReadOnly,
            IProdutoSkuReadOnlyRepository produtoSkuReadOnly,
            IProdutoReadOnlyRepository produtoReadOnly)
        {
            _produtoImagemReadOnly = produtoImagemReadOnly;
            _produtoSkuReadOnly = produtoSkuReadOnly;
            _produtoReadOnly = produtoReadOnly;
            _logger = logger;

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("[controller]/[action]/{id?}/{produto?}")]
        public IActionResult Detalhes(int id)
        {
            var prod = _produtoReadOnly.RecuperarParaDetalhes(id);
            if (prod == null)
                return NotFound();

            var skus = _produtoSkuReadOnly.ListarComEstoque(prod.Id, false);
            var imagens = _produtoImagemReadOnly.ListarPorId(prod.Id);
            var vm = new ProdutoViewModel(prod);
            vm.FillSkus(skus);
            vm.FillImagens(imagens);
            return View(vm);
        }
    }
}

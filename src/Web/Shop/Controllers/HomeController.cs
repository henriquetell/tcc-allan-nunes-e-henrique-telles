using ApplicationCore.Interfaces.Logging;
using ApplicationCore.Respositories.ReadOnly;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Extenders;
using Framework.UI.Extenders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Resources;
using Shop.ViewModels.FaleConosco;
using Shop.ViewModels.Produto;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : BaseController
    {
        private readonly IAppLogger<HomeController> _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly IProdutoReadOnlyRepository _produtoReadOnly;
        private readonly IProdutoImagemReadOnlyRepository _produtoImagemReadOnly;
        private readonly IProdutoSkuReadOnlyRepository _produtoSkuReadOnly;
        private readonly EmailService _emailService;

        public HomeController(
            IAppLogger<HomeController> logger,
            UrlEncoder urlEncoder,
            EmailService emailService,
            IProdutoReadOnlyRepository produtoReadOnly)
        {
            _produtoReadOnly = produtoReadOnly;
            _emailService = emailService;
            _urlEncoder = urlEncoder;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var vm = _produtoReadOnly.ListarDestaque().Select(p => new ProdutoViewModel(p)).ToList();
            return View(vm);
        }

    }
}

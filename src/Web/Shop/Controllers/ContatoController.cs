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
    public class ContatoController : BaseController
    {
        private readonly IAppLogger<HomeController> _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly IProdutoReadOnlyRepository _produtoReadOnly;
        private readonly IProdutoImagemReadOnlyRepository _produtoImagemReadOnly;
        private readonly IProdutoSkuReadOnlyRepository _produtoSkuReadOnly;
        private readonly EmailService _emailService;

        public ContatoController(
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

       
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(FaleConoscoViewModel vm)
        {
            try
            {
                var isAjax = Request.IsAjaxRequest();

                if (!ModelState.IsValid)
                {
                    if (isAjax)
                        return BadRequest(ModelStateResource.Invalido);
                    else
                        ExibirMensagemErro(ModelStateResource.Invalido);
                }
                else
                {
                    await _emailService.EnviarFaleConosco(vm.Nome, vm.Email, vm.Telefone, vm.Mensagem);

                    if (isAjax)
                        return Ok(MensagemResource.SucessoFaleConosco);
                    else
                        ExibirMensagemSucesso(MensagemResource.SucessoFaleConosco);
                }
            }
            catch (MensagemException ex)
            {
                AppLogger.Exception(ex);
                return BadRequest(ex.GetMessages());
            }
            catch (Exception ex)
            {
                AppLogger.Exception(ex);
                return BadRequest(MensagemResource.Erro);

            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index() => View();
    }
}

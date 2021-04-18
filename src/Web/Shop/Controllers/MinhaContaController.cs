using ApplicationCore.Interfaces.Logging;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.UI.Extenders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Resources;
using Shop.ViewModels.MinhaConta;
using System;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [Route("[controller]/[action]")]
    public class MinhaContaController : BaseController
    {
        private readonly IAppLogger<MinhaContaController> _logger;
        private readonly ClienteService _clienteService;
        public MinhaContaController(
            IAppLogger<MinhaContaController> logger,
            ClienteService clienteService)
        {
            _clienteService = clienteService;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var cliente = _clienteService.Recuperar(Usuario.Identificao.Id);
            var model = new MinhaContaViewModel();
            model.Fill(cliente);
            model.Endereco.Fill(cliente.Endereco);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MinhaContaViewModel vm)
        {
            try
            {
                ModelState.RemoverIf<MinhaContaViewModel>(m => m.Senha, !vm.AlterarSenha);
                ModelState.RemoverIf<MinhaContaViewModel>(m => m.NovaSenha, !vm.AlterarSenha);
                ModelState.RemoverIf<MinhaContaViewModel>(m => m.ConfirmarNovaSenha, !vm.AlterarSenha);

                if (!ModelState.IsValid)
                    return View(vm);

                ModelState.Clear();

                await _clienteService.AtualizarCliente(Usuario.Identificao.Id, vm.GetDataValue());

                return RedirectToAction(nameof(Index));
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex);
                AppLogger.Exception(ex);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(MensagemResource.Erro);
                AppLogger.Exception(ex);
            }

            return View(vm);
        }
    }
}

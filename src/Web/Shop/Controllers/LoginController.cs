using ApplicationCore.Interfaces.Logging;
using ApplicationCore.Services;
using Framework.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Resources;
using Shop.ViewModels.Login;
using System;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    public class LoginController : BaseController
    {
        private readonly IAppLogger<LoginController> _logger;
        private readonly ClienteService _clienteService;
        public LoginController(
            IAppLogger<LoginController> logger, ClienteService clienteService)
        {
            _clienteService = clienteService;
            _logger = logger;

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Entrar(string returnUrl = null)
        {
            await SignOutAsync();
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entrar(LoginViewModel vm, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(vm);

                ModelState.Clear();

                var model = _clienteService.Autenticar(vm.Email, vm.Senha);

                await AutenticarUsuarioAsync(new UsuarioAuthModel(model), vm.LembrarMe);

                ViewData["ReturnUrl"] = returnUrl;

                if (!string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Registrar()
        {
            await SignOutAsync();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistrarViewModel vm, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(vm);

                ModelState.Clear();

                var model = await _clienteService.Registrar(vm.Documento, vm.Nome, vm.DataNascimento.Value, vm.Email, vm.Senha, vm.ConfirmarSenha);

                await AutenticarUsuarioAsync(new UsuarioAuthModel(model));

                ViewData["ReturnUrl"] = returnUrl;

                if (!string.IsNullOrEmpty(returnUrl) &&
                    returnUrl.IndexOf("checkout", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    ViewData["ReturnUrl"] = "/Basket/Index";
                }

                return RedirectToAction("Index", "Home");
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Sair()
        {
            await SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

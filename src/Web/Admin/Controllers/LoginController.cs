using Admin.Filters;
using Admin.Resources;
using Admin.ViewModels.Login;
using Admin.ViewModels.Usuario;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Extenders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class LoginController : BaseController
    {
        private UsuarioService UsuarioService => GetService<UsuarioService>();

        [AuthUsuarioFilter(false)]
        public IActionResult Index() => View();

        [HttpPost]
        [AuthUsuarioFilter(false)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            try
            {
                ModelState.Clear();

                var model = UsuarioService.Autenticar(vm?.Email, vm?.Senha);

                await AutenticarUsuarioAsync(new UsuarioAuthViewModel(model));

                ExibirMensagemInformacao(MensagemResource.BemVindo.FormatText(model.Nome));

                return RedirectToAction(nameof(Index), "Home");
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

        [AuthUsuarioFilter(false)]
        public IActionResult Sair()
        {
            DesautenticarUsuarioAsync();
            return RedirectToAction(nameof(Index), "Login");
        }


        [HttpPost]
        [AuthUsuarioFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaViewModel vm)
        {
            try
            {
                ModelState.Clear();

                await UsuarioService.AlterarSenha(Usuario.Identificao.Id, vm?.SenhaAtual, vm?.NovaSenha, vm?.ConfirmarNovaSenha, ConteudoResource.EmailRecuperacao)
                            .ConfigureAwait(true);

                ExibirMensagemSucesso(MensagemResource.SenhaAlteradaSucesso);
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

            if (!string.IsNullOrWhiteSpace(vm?.ReturnUrl))
                return Redirect(vm.ReturnUrl);
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
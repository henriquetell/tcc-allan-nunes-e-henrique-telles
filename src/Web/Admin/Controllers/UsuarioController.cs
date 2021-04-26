using Admin.Filters;
using Admin.Resources;
using Admin.Services;
using Admin.ViewModels.Usuario;
using ApplicationCore.Entities;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Security.Authorization;
using Framework.Security.Permissoes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class UsuarioController : BaseController
    {
        private UsuarioServiceWeb UsuarioServiceWeb => GetService<UsuarioServiceWeb>();
        private UsuarioService UsuarioService => GetService<UsuarioService>();


        [AuthUsuarioFilter(UsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Index(UsuarioFiltroViewModel filtro) => View(UsuarioServiceWeb.Listar(filtro));

        [AuthUsuarioFilter(UsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Form(int id) => View(UsuarioServiceWeb.Recuperar(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(UsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public async Task<IActionResult> Form(UsuarioViewModel vm)
        {
            try
            {
                if (vm == null || !ModelState.IsValid)
                {
                    ExibirMensagemErro(MensagemResource.ModelStateInvalido);
                    return View(vm);
                }

                var model = new UsuarioEntity
                {
                    Id = vm.Id,
                    Nome = vm.Nome,
                    Email = vm.Email,
                    Status = vm.Status,
                    IdGrupoUsuario = vm.IdGrupoUsuario
                };

                await UsuarioService
                    .Salvar(model, ConteudoResource.EmailSenha)
                    .ConfigureAwait(true);

                ExibirMensagemSucesso(MensagemResource.Sucesso);

                return RedirectToAction(nameof(Form), new { model.Id });
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex);

                return View(vm);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(MensagemResource.Erro);

                AppLogger.Exception(ex);

                return RedirectToAction(nameof(Form));
            }
        }

        [HttpPost]
        [AuthUsuarioFilter]
        public IActionResult Imagem(string imagem)
        {
            try
            {
                UsuarioService.SalvarImagem(Usuario.Identificao.Id, imagem);

                return Ok(true);
            }
            catch (MensagemException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                AppLogger.Exception(ex);
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(UsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Excluir(int id)
        {
            try
            {
                UsuarioService.Excluir(id);

                ExibirMensagemSucesso(MensagemResource.ExcluirSucesso);
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(
                    ErroRelacionamento(ex)
                    ? MensagemResource.ErroRelacionamento
                    : MensagemResource.Erro);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
using Admin.Filters;
using Admin.Resources;
using Admin.Services;
using Admin.ViewModels.GrupoUsuario;
using ApplicationCore.Entities;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Security.Authorization;
using Framework.Security.Permissoes;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Admin.Controllers
{
    public class GrupoUsuarioController : BaseController
    {
        private GrupoUsuarioServiceWeb GrupoAcessoServiceWeb => GetService<GrupoUsuarioServiceWeb>();
        private GrupoUsuarioService GrupoUsuarioServico => GetService<GrupoUsuarioService>();

        [AuthUsuarioFilter(GrupoUsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Leitura)]
        public IActionResult Index(GrupoUsuarioFiltroViewModel filtro) => View(GrupoAcessoServiceWeb.Listar(filtro));

        [AuthUsuarioFilter(GrupoUsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Leitura)]
        public IActionResult Form(int id) => View(GrupoAcessoServiceWeb.Recuperar(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(GrupoUsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Escrever)]
        public IActionResult Form(GrupoUsuarioViewModel vm)
        {
            try
            {
                if (vm == null || !ModelState.IsValid)
                {
                    ExibirMensagemErro(MensagemResource.ModelStateInvalido);
                    return View(vm);
                }

                var model = new GrupoUsuarioEntity
                {
                    Id = vm.Id,
                    Nome = vm.Nome,
                    Status = vm.Status
                };

                GrupoUsuarioServico.Salvar(model, vm.PermissoesDoGrupo);

                ExibirMensagemSucesso(MensagemResource.Sucesso);

                return RedirectToAction(nameof(Form), new { model.Id });
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

            return RedirectToAction(nameof(Form), new { vm?.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(GrupoUsuarioPermissoes.Gerenciar, AuthPermissaoTipoAcao.Excluir)]
        public IActionResult Excluir(int id)
        {
            try
            {
                GrupoUsuarioServico.Excluir(id);

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
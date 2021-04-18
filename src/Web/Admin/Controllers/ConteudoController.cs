using Admin.Filters;
using Admin.Resources;
using Admin.Services;
using Admin.ViewModels.Conteudo;
using ApplicationCore.Entities;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Extenders;
using Framework.Security.Authorization;
using Framework.Security.Permissoes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class ConteudoController : BaseController
    {
        private ConteudoServiceWeb ConteudoServiceWeb => GetService<ConteudoServiceWeb>();
        private ConteudoService ConteudoService => GetService<ConteudoService>();

        [AuthUsuarioFilter(ConteudoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Leitura)]
        public IActionResult Index(ConteudoFiltroViewModel filtro)
        {
            ModelState.Clear();
            return View(ConteudoServiceWeb.Listar(filtro));
        }

        [AuthUsuarioFilter(ConteudoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Leitura)]
        public IActionResult Form(int id) => View(ConteudoServiceWeb.Recuperar(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ConteudoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Escrever)]
        public IActionResult Form(ConteudoViewModel vm)
        {
            try
            {
                if (vm == null || !ModelState.IsValid)
                {
                    ExibirMensagemErro(MensagemResource.ModelStateInvalido);
                    return View(vm);
                }

                var model = new ConteudoEntity
                {
                    Id = vm.Id,
                    Status = vm.Status,
                    Titulo = vm.Titulo,
                    Descricao = vm.Descricao,
                    DescricaoComplemento = vm.DescricaoComplemento,
                    Assunto = vm.Assunto,
                    IdConteudo = vm.IdConteudoChave.GetValueOrDefault()
                };

                ConteudoService.Salvar(model);

                ExibirMensagemSucesso(MensagemResource.Sucesso);

                return RedirectToAction(nameof(Form), new { model.Id });
            }
            catch (MensagemException ex)
            {
                ExibirMensagemErro(ex);
                AppLogger.Exception(ex);
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
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ConteudoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Escrever)]
        public async Task<IActionResult> UploadAnexoAsync(ConteudoAnexoViewModel vm)
        {
            try
            {
                if (vm == null || vm.IdConteudo <= 0)
                    return NotFound();

                if (vm.Arquivo == null)
                    return BadRequest(MensagemResource.ArquivoNaoEnviado);

                var extensao = RecuperarExtensaoDoArquivo(vm.Arquivo);

                await ConteudoService
                    .SalvarAnexoAsync(vm.IdConteudo, vm.Arquivo.OpenReadStream(), vm.Arquivo.FileName, extensao)
                    .ConfigureAwait(true);

                return Ok(MensagemResource.Sucesso);
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
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ConteudoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Excluir)]
        public async Task<IActionResult> ExcluirAnexoAsync(int id)
        {
            try
            {
                var idConteudo = await ConteudoService
                    .ExcluirAnexoAsync(id)
                    .ConfigureAwait(true);

                ExibirMensagemSucesso(MensagemResource.ExcluirSucesso);

                return RedirectToAction(nameof(Form), new { id = idConteudo });
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
using Admin.Filters;
using Admin.Resources;
using Admin.Services;
using Admin.ViewModels.Produto;
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
    public class ProdutoController : BaseController
    {
        private ProdutoServiceWeb ProdutoServiceWeb => GetService<ProdutoServiceWeb>();
        private ProdutoService ProdutoService => GetService<ProdutoService>();

        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Index(ProdutoFiltroViewModel filtro) => View(ProdutoServiceWeb.Listar(filtro));

        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Form(int id) => View(ProdutoServiceWeb.Recuperar(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public async Task<IActionResult> Form(ProdutoViewModel vm)
        {
            try
            {
                if (vm == null || !ModelState.IsValid)
                {
                    ExibirMensagemErro(MensagemResource.ModelStateInvalido);
                    return View(vm);
                }

                if (string.IsNullOrWhiteSpace(vm.ImagemUrl) && vm.Imagem == null)
                {
                    ExibirMensagemErro(MensagemResource.ImagemNaoInformada);
                    return View(vm);
                }

                var model = new ProdutoEntity
                {
                    Id = vm.Id,
                    IdConteudo = vm.IdConteudo ?? throw new InvalidOperationException("IdConteudo NULL"),
                    Titulo = vm.Titulo.GetFirsts(1000),
                    Codigo = vm.Codigo.GetFirsts(2000),
                    DescricaoLonga = vm.DescricaoLonga,
                    Status = vm.Status ?? throw new InvalidOperationException("Status NULL"),
                    CategoriaProduto = vm.CategoriaProduto ?? ApplicationCore.Enuns.ECategoriaProduto.ProdutoFisico,
                };

                if (vm.Imagem != null)
                {
                    model.Imagem = $"{Guid.NewGuid()}{RecuperarExtensaoDoArquivo(vm.Imagem)}";
                    await ProdutoService.SalvarImagemAsync(vm.Imagem.OpenReadStream(), model.Imagem);
                }

                ProdutoService.Salvar(model);

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

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public IActionResult Excluir(int id)
        {
            try
            {
                ProdutoService.Excluir(id);

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Permitir)]
        public async Task<IActionResult> EnviarNps(ProdutoNpsViewModel vm)
        {
            try
            {

                if (vm == null || vm?.IdProduto <= 0)
                    return NotFound();

                if (vm.Email == null)
                {
                    ExibirMensagemErro(MensagemResource.EmailNaoInformado);
                    return RedirectToAction(nameof(Form), new { id = vm.IdProduto });
                }

                if (vm.DataLimite.HasValue && vm.DataLimite.Value.Date < DateTime.Now.Date)
                {
                    ExibirMensagemErro(MensagemResource.DataLimiteNpsInvalida);
                    return RedirectToAction(nameof(Form), new { id = vm.IdProduto });
                }

                await ProdutoService.EnviarNps(vm.IdProduto.Value, vm.Email, vm.DataLimite);

                ExibirMensagemSucesso(MensagemResource.Sucesso);

                return RedirectToAction(nameof(Form), new { id = vm.IdProduto });
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
    }
}
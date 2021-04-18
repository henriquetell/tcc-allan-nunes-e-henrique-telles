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
using Framework.UI.Extenders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Controllers
{
    public class ProdutoController : BaseController
    {
        private ProdutoServiceWeb ProdutoServiceWeb => GetService<ProdutoServiceWeb>();
        private ProdutoService ProdutoService => GetService<ProdutoService>();

        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Leitura)]
        public IActionResult Index(ProdutoFiltroViewModel filtro) => View(ProdutoServiceWeb.Listar(filtro));

        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Leitura)]
        public IActionResult Form(int id) => View(ProdutoServiceWeb.Recuperar(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Escrever)]
        public IActionResult Form(ProdutoViewModel vm)
        {
            try
            {
                if (vm == null || !ModelState.IsValid)
                {
                    ExibirMensagemErro(MensagemResource.ModelStateInvalido);
                    return View(vm);
                }

                var model = new ProdutoEntity
                {
                    Id = vm.Id,
                    Titulo = vm.Titulo.GetFirsts(1000),
                    Codigo = vm.Codigo.GetFirsts(2000),
                    DescricaoCurta = vm.DescricaoCurta.GetFirsts(3000),
                    DescricaoLonga = vm.DescricaoLonga,
                    Preco = vm.Preco ?? 0,
                    Status = vm.Status ?? ApplicationCore.Enuns.EStatus.Pendente,
                    CategoriaProduto = vm.CategoriaProduto ?? ApplicationCore.Enuns.ECategoriaProduto.ProdutoFisico,
                };

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
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Excluir)]
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

        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Leitura)]
        public JsonResult ListarImagens(int id) => Json(ProdutoServiceWeb.Listar(id));

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Escrever)]
        public async Task<IActionResult> UploadImagem(ProdutoImagemViewModel vm)
        {
            try
            {

                if (vm == null || vm.IdProduto <= 0)
                    return NotFound();

                if (vm.Arquivo == null)
                    return BadRequest(MensagemResource.ArquivoNaoEnviado);

                await ProdutoService
                    .SalvarImagemAsync(vm.IdProduto, vm.Arquivo.OpenReadStream(), RecuperarExtensaoDoArquivo(vm.Arquivo))
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
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Escrever)]
        public IActionResult OrdernarImagens(int[] imagem)
        {
            try
            {
                if (imagem == null || !imagem.Any())
                    return NotFound();

                var idProduto = ProdutoService.OrdernarImagens(imagem.ToList());

                ExibirMensagemSucesso(MensagemResource.Sucesso);

                return RedirectToAction(nameof(Form), new { id = idProduto });
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Excluir)]
        public async Task<IActionResult> ExcluirImagem(int id)
        {
            try
            {
                var idProduto = await ProdutoService
                    .ExcluirImagemAsync(id)
                    .ConfigureAwait(true);

                ExibirMensagemSucesso(MensagemResource.ExcluirSucesso);

                return RedirectToAction(nameof(Form), new { id = idProduto });
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
        [AuthUsuarioFilter(ProdutoPermissoes.Gerenciar, AuthPermissaoTipoAcao.Escrever)]
        public IActionResult SalvarSku(ProdutoSkuViewModel vm)
        {
            try
            {
                if (vm == null)
                    return RedirectToAction(nameof(Index));

                ModelState.RemoverIf<ProdutoSkuViewModel>(c => c.Lancamento, !vm.MovimentarEstoque);
                ModelState.RemoverIf<ProdutoSkuViewModel>(c => c.Quantidade, !vm.MovimentarEstoque);

                if (ModelState.IsValid)
                {
                    var model = new ProdutoSkuEntity
                    {
                        Id = vm.IdProdutoSku,
                        IdProduto = vm.IdProduto,
                        Descricao = vm.Descricao,
                        TipoSku = vm.TipoSku,
                        Status = vm.Status
                    };

                    ProdutoService.Salvar(model);

                    ExibirMensagemSucesso(MensagemResource.Sucesso);
                }
                else
                    ExibirMensagemErro(MensagemResource.ModelStateInvalido);
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

            return RedirectToAction(nameof(Form), new { id = vm?.IdProduto });
        }
    }
}
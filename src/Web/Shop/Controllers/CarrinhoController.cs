using ApplicationCore.Respositories.ReadOnly;
using ApplicationCore.Services;
using Framework.Exceptions;
using Framework.Extenders;
using Framework.UI.Extenders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Resources;
using Shop.ViewModels.Carrinho;
using System;
using System.Linq;

namespace Shop.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CarrinhoController : BaseController
    {
        private ICarrinhoItemReadOnlyRepository CarrinhoItemReadOnlyRepository => GetService<ICarrinhoItemReadOnlyRepository>();
        private CarrinhoService CarrinhoService => GetService<CarrinhoService>();

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var vm = new CarrinhoViewModel();
            if (CarrinhoService.ValidarCarrinho(Usuario.Identificao.Id, null, false, out var carrinho))
            {
                var itens = CarrinhoItemReadOnlyRepository.ListarItensEmAberto(Usuario.Identificao.Id);
                vm.IdCarrinho = carrinho.Id;
                vm.Itens = itens.Select(ci => new CarrinhoItemViewModel(ci)).ToList();
            }
            return View(vm);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CarrinhoViewModel vm)
        {
            if (CarrinhoService.ValidarCarrinho(Usuario.Identificao.Id, null, false, out var carrinho))
                return RedirectToAction(nameof(Index), "Checkout");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Remover(RemoverProdutoViewModel vm)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                    return RedirectToAction(nameof(Index));

                if (!ModelState.IsValid)
                    return BadRequest(ModelStateResource.Invalido);

                AppLogger.Info($"Cliente: {Usuario.Identificao.Id} removeu o item: {vm.IdCarrinhoItem}");

                CarrinhoService.Remover(Usuario.Identificao.Id, vm.IdCarrinho.Value, vm.IdCarrinhoItem.Value, vm.IdProdutoSku.Value);

                return Ok();
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
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public IActionResult AdicionarProduto(AdicionarProdutoViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AppLogger.Info($"Cliente: {Usuario.Identificao.Id} Adicionou o item: {vm?.Id}");
                    CarrinhoService.AdicionarProduto(Usuario.Identificao.Id, vm.IdProdutoSku.GetValueOrDefault(), vm.Quantidade ?? 1);
                    return RedirectToAction(nameof(Index));
                }
                else
                    ExibirMensagemErro(ModelStateResource.Invalido);
            }
            catch (MensagemException ex)
            {
                AppLogger.Exception(ex);
                ExibirMensagemErro(ex.GetMessages());
            }
            catch (Exception ex)
            {
                AppLogger.Exception(ex);
                ExibirMensagemErro(MensagemResource.Erro);
            }

            return vm.Id > 0 && !string.IsNullOrWhiteSpace(vm.Titulo)
                ? RedirectToAction(nameof(ProdutoController.Detalhes), "Produto", new { vm.Id, produto = vm.Titulo.ToUrlFriendly() })
                : RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [Authorize]
        public IActionResult DiminuirProduto(AlterarQuantidadeViewModel vm) => AlterarQuantidade(vm, -1);

        [HttpPost]
        [Authorize]
        public IActionResult AumentarProduto(AlterarQuantidadeViewModel vm) => AlterarQuantidade(vm, 1);

        [NonAction]
        private IActionResult AlterarQuantidade(AlterarQuantidadeViewModel vm, int quantidade)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                    return RedirectToAction(nameof(Index));

                if (!ModelState.IsValid)
                    return BadRequest(ModelStateResource.Invalido);

                AppLogger.Info($"Cliente: {Usuario.Identificao.Id} {(quantidade > 0 ? "aumentou" : "diminuiu")} o item: {vm.IdCarrinhoItem}");

                CarrinhoService.AlterarQuantidade(Usuario.Identificao.Id, vm.IdCarrinho.Value, vm.IdCarrinhoItem.Value, vm.IdProdutoSku.Value, quantidade);

                return Ok();
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

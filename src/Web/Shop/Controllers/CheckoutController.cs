using ApplicationCore.DataValue;
using ApplicationCore.Enuns;
using ApplicationCore.Respositories.ReadOnly;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.ViewModels.Carrinho;
using Shop.ViewModels.Checkout;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CheckoutController : BaseController
    {
        private ICarrinhoItemReadOnlyRepository CarrinhoItemReadOnlyRepository => GetService<ICarrinhoItemReadOnlyRepository>();
        private CarrinhoService CarrinhoService => GetService<CarrinhoService>();
        private CheckoutService CheckoutService => GetService<CheckoutService>();


        [HttpGet]
        [Authorize]
        [Route("[controller]/[action]/{idCarrinho?}")]
        public async Task<IActionResult> Index(int? idCarrinho)
        {
            if (CarrinhoService.ValidarCarrinho(Usuario.Identificao.Id, idCarrinho, false, out var carrinho))
            {
                var itens = idCarrinho.HasValue
                     ? CarrinhoItemReadOnlyRepository.ListarItensPorCarrinho(idCarrinho.Value)
                     : CarrinhoItemReadOnlyRepository.ListarItensEmAberto(Usuario.Identificao.Id);

                var vm = new CheckoutViewModel
                {
                    IdCarrinho = idCarrinho,
                    ValorTotal = itens.Sum(ci => (ci.PrecoPor + ci.ValorTaxa.GetValueOrDefault() + ci.ValorFrete.GetValueOrDefault()) * ci.Quantidade),
                    QuantidadeMaximaParcelamento = itens.Max(ci => ci.QuantidadeMaximaParcelamento),
                    Itens = itens.Select(ci => new CarrinhoItemViewModel(ci))
                };
                vm.Pagamento.IdSessao = await CheckoutService.CriarSessaoAsync();
                return View(vm);
            }
            return RedirectToAction("Index", "Carrinho");
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel vm)
        {
            var (_, statusPedido) = await CheckoutService.RealizarCheckoutAsync(Usuario.Identificao.Id, vm.IdCarrinho, new CheckoutPagamentoDataValue
            {
                Bandeira = vm.Pagamento.Bandeira,
                Cvv = vm.Pagamento.Cvv,
                MesValidade = vm.Pagamento.MesValidade,
                AnoValidade = vm.Pagamento.AnoValidade,
                NomeCartao = vm.Pagamento.NomeCartao,
                NumeroCartao = vm.Pagamento.NumeroCartao,
                QuantidadeParcelas = vm.Pagamento.QuantidadeParcelas.GetValueOrDefault(),
                CardToken = vm.Pagamento.CardToken,
                Hash = vm.Pagamento.Hash,
                DocumentoTitularCartao = vm.Pagamento.DocumentoTitularCartao,
                DataNascimentoTitularCartao = vm.Pagamento.DataNascimentoTitularCartao ?? DateTime.MinValue,
                TelefoneTitularCartao = vm.Pagamento.TelefoneTitularCartao
            });

            if (statusPedido == EStatusPedido.Liberado ||
                statusPedido == EStatusPedido.ProcesandoPagamento)
                return RedirectToAction(nameof(Sucesso));
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Sucesso()
        {
            return View();
        }
    }
}

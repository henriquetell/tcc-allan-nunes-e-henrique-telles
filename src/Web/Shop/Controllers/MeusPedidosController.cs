using ApplicationCore.Respositories.ReadOnly;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.ViewModels.Pedido;
using System.Linq;

namespace Shop.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [Route("[controller]/[action]")]
    public class MeusPedidosController : BaseController
    {
        private IPedidoReadOnlyRepository PedidoReadOnlyRepository => GetService<IPedidoReadOnlyRepository>();
        private IPedidoItemReadOnlyRepository PedidoItemReadOnlyRepository => GetService<IPedidoItemReadOnlyRepository>();
        public MeusPedidosController()
        { }

        [HttpGet]
        public IActionResult Index()
        {
            var vm = PedidoReadOnlyRepository.ListarParaMeusPedidos(Usuario.Identificao.Id).Select(p => new PedidoDetalhesViewModel(p)).ToList();
            return View(vm);
        }

        [HttpGet]
        [Route("{idPedido}")]
        [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 30)]
        public IActionResult Detalhes(int idPedido)
        {
            var pedido = PedidoReadOnlyRepository.ListarParaMeusPedidosDetalhes(idPedido, Usuario.Identificao.Id);
            var itens = PedidoItemReadOnlyRepository.Listar(idPedido);
            return View(new PedidoDetalhesViewModel(pedido)
            {
                Itens = itens.Select(pi => new PedidoItemDetalhesViewModel(pi))
            });
        }
    }
}

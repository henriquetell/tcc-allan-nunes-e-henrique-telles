using ApplicationCore.Respositories.ReadOnly;
using Framework.UI.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shop.Models;

namespace Shop.ViewComponents
{
    public class CarrinhoViewComponent : ViewComponent
    {
        private readonly ICarrinhoReadOnlyRepository _carrinhoReadOnlyRepository;
        public CarrinhoViewComponent(ICarrinhoReadOnlyRepository carrinhoReadOnlyRepository)
        {
            _carrinhoReadOnlyRepository = carrinhoReadOnlyRepository;
        }

        public IViewComponentResult Invoke()
        {
            var usuario = HttpContext?.RequestServices.GetService<AuthUsuario<UsuarioAuthModel>>();
            var vm  = usuario.Autenticado 
                ? _carrinhoReadOnlyRepository.TotalEmAberto(usuario.Identificao.Id) 
                : 0;
            return View(vm);
        }
    }
}

using Admin.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class HomeController : BaseController
    {
        [AuthUsuarioFilter]
        public IActionResult Index() => View();
    }
}

using Framework.UI.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseApiController : FrameworkControllerBase<UsuarioAuthModel>
    { }
}

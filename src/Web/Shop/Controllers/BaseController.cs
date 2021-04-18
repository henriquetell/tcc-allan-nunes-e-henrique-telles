using ApplicationCore.Extenders;
using ApplicationCore.Interfaces.Logging;
using Framework.UI.MVC.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Shop.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public abstract class BaseController : FrameworkControllerBase<UsuarioAuthModel>
    {
        protected static bool ErroRelacionamento(Exception ex) => ex?.InnerException?.Message?.Contains(
                       "The DELETE statement conflicted with the REFERENCE constraint") ?? false;

        protected static string RecuperarExtensaoDoArquivo(IFormFile formFile) => Path.GetExtension(
            formFile.ContentDisposition.Split(new[] { "filename=" }, StringSplitOptions.None)[1].Replace("\"", ""));

        protected IAppLogger AppLogger => HttpContext.RequestServices.AppLogger(GetType());

        protected TService GetService<TService>() => (TService) HttpContext.RequestServices.GetService(typeof(TService));

        protected async Task SignOutAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
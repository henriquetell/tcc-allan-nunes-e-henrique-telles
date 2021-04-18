using Admin.ViewModels.Usuario;
using ApplicationCore.Extenders;
using ApplicationCore.Interfaces.Logging;
using Framework.UI.MVC.Controllers;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Admin.Controllers
{
    public abstract class BaseController : FrameworkControllerBase<UsuarioAuthViewModel>
    {
        protected static bool ErroRelacionamento(Exception ex) => ex?.InnerException?.Message?.Contains(
                       "The DELETE statement conflicted with the REFERENCE constraint", StringComparison.InvariantCultureIgnoreCase) ?? false;

        protected static string RecuperarExtensaoDoArquivo(IFormFile formFile) => Path.GetExtension(
            formFile?.ContentDisposition?.Split(new[] { "filename=" }, StringSplitOptions.None)[1].Replace("\"", "", StringComparison.InvariantCultureIgnoreCase));

        protected IAppLogger AppLogger => HttpContext.RequestServices.AppLogger(GetType());

        protected TService GetService<TService>() => (TService)HttpContext.RequestServices.GetService(typeof(TService));
    }
}
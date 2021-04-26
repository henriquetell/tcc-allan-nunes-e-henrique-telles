using Admin.ViewModels.Usuario;
using Framework.Security.Authorization;
using Framework.UI.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Admin.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AuthUsuarioFilter : ActionFilterAttribute
    {
        public bool RequerAutenticacao { get; set; }

        public Guid IdPermissao { get; set; }

        public AuthPermissaoTipoAcao Acao { get; set; }

        public AuthUsuarioFilter(bool requerAutenticacao = true)
        {
            RequerAutenticacao = requerAutenticacao;
        }

        public AuthUsuarioFilter(string idPermissao, AuthPermissaoTipoAcao acao)
            : this()
        {
            if (!Guid.TryParse(idPermissao, out var id))
                throw new Exception($"O ID Funcionalidade {idPermissao} não é válido");

            IdPermissao = id;
            Acao = acao;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var usuario = filterContext.HttpContext.RequestServices.GetService<AuthUsuario<UsuarioAuthViewModel>>();

            if (!RequerAutenticacao)
            {
                if (usuario.Autenticado &&
                    filterContext.RouteData.Values["controller"] as string == "Login" &&
                    filterContext.RouteData.Values["action"] as string == "Index")
                    filterContext.Result = new RedirectResult("/Home");

                base.OnActionExecuting(filterContext);
            }
            else
            {
                if (!usuario.Autenticado)
                        filterContext.Result = new UnauthorizedResult();
                else if (IdPermissao != Guid.Empty && !usuario.ValidarAcesso(IdPermissao, Acao))
                    RedirectToLogin(filterContext, true);
                else
                    base.OnActionExecuting(filterContext);
            }
        }

        private static void RedirectToLogin(ActionExecutingContext filterContext, bool semAcesso = false, string url = null)
        {
            var urlQueryString = new List<string>();
            if (semAcesso)
                urlQueryString.Add($"{nameof(semAcesso)}={semAcesso}");

            urlQueryString.Add($"href={filterContext.HttpContext.Request.Path}");

            url += string.Join("&", urlQueryString.ToArray());

            if (filterContext.HttpContext.Request.Headers["X-Requested-With"].Count > 0)
            {
                filterContext.HttpContext.Response.Headers.Add(nameof(RedirectToLogin), url);
                filterContext.Result = new UnauthorizedResult();
            }
            else
            {
                var result = new RedirectResult(url);
                filterContext.Result = result;
            }
        }
    }
}

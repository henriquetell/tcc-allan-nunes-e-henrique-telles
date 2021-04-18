using Framework.UI.Security.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Framework.UI.Extenders
{
    public static class AuthUsuarioExtenders
    {
        public static async Task<AuthUsuario<TIdentificacao>> AutenticarAsync<TIdentificacao>(this HttpContext httpContext, TIdentificacao identificao, bool lembrarMe)
            where TIdentificacao : IAuthUsuarioIdentificacao
        {
            var user = new AuthUsuario<TIdentificacao>(identificao);
            var claims = new List<Claim>();
            user.FillClaims(claims);

            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var property = new AuthenticationProperties
            {
                IsPersistent = lembrarMe,
                ExpiresUtc = lembrarMe ? DateTime.UtcNow.AddDays(5) : DateTime.UtcNow.AddHours(1)
            };

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), property);

            return user;
        }

        public static AuthUsuario<TIdentificacao> GetAuthUsuario<TIdentificacao>(this HttpContext httpContext)
            where TIdentificacao : IAuthUsuarioIdentificacao
        {
            var claims = httpContext?.User?.Claims;
            if (claims == null || !claims.Any())
                return new AuthUsuario<TIdentificacao>();

            return new AuthUsuario<TIdentificacao>(claims);
        }

        public static AuthUsuario GetAuthUsuario(this HttpContext httpContext)
        {
            var claims = httpContext.User.Claims;
            return new AuthUsuario(claims.Any());
        }

        public static int GetLastUsuario(this HttpContext httpContext)
        {
            int.TryParse(httpContext.Request.Cookies["asp-usuario"], out var idUsuario);
            return idUsuario;
        }

        public static Task DesautenticarAsync(this HttpContext httpContext) => httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}

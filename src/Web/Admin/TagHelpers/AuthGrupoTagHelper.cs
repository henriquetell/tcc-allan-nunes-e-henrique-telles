using Admin.ViewModels.Usuario;
using Framework.UI.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.TagHelpers
{
    [HtmlTargetElement(HtmlTargetElementAttribute.ElementCatchAllTarget, Attributes = AuthGrupoAttributeName)]
    public class AuthGrupoTagHelper : TagHelper
    {
        private const string AuthGrupoAttributeName = "asp-auth-grupo";

        [HtmlAttributeName(AuthGrupoAttributeName)]
        public string[] IdPermissao { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthGrupoTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var usuario = _httpContextAccessor.HttpContext.RequestServices.GetService<AuthUsuario<UsuarioAuthViewModel>>();

            if (!usuario.Autenticado || !usuario.ValidarAcessoGrupo(IdPermissao))
                output.SuppressOutput();
        }
    }
}

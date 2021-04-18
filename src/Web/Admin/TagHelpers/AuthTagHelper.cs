using Admin.ViewModels.Usuario;
using Framework.Security.Authorization;
using Framework.UI.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.TagHelpers
{
    [HtmlTargetElement(HtmlTargetElementAttribute.ElementCatchAllTarget, Attributes = AuthFuncionalidadeAttributeName)]
    public class AuthTagHelper : TagHelper
    {
        private const string AuthFuncionalidadeAttributeName = "asp-auth-perm";

        [HtmlAttributeName(AuthFuncionalidadeAttributeName)]
        public string IdPermissao { get; set; }

        private const string AuthAcaoAttributeName = "asp-auth-acao";

        [HtmlAttributeName(AuthAcaoAttributeName)]
        public AuthPermissaoTipoAcao Acao { get; set; } = AuthPermissaoTipoAcao.Leitura;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var usuario = _httpContextAccessor.HttpContext.RequestServices.GetService<AuthUsuario<UsuarioAuthViewModel>>();

            if (!usuario.Autenticado || !usuario.ValidarAcesso(IdPermissao, Acao))
                output.SuppressOutput();
        }
    }
}

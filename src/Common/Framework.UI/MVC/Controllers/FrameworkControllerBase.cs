using Framework.Exceptions;
using Framework.Extenders;
using Framework.UI.Extenders;
using Framework.UI.Mensagem;
using Framework.UI.Security.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Framework.UI.MVC.Controllers
{
    public class FrameworkControllerBase<TUsuarioIdentificao> : Controller
           where TUsuarioIdentificao : IAuthUsuarioIdentificacao
    {

        public AuthUsuario<TUsuarioIdentificao> Usuario => HttpContext?.RequestServices.GetService<AuthUsuario<TUsuarioIdentificao>>();

        #region Mensagens

        [NonAction]
        protected void ExibirMensagemErro(string mensagem, string titulo = "OPS!") => ExibirMensagem(ETipoMensagem.Erro, mensagem, titulo);

        [NonAction]
        protected void ExibirMensagemErro(MensagemException mensagemException, string titulo = "OPS!") => ExibirMensagem(mensagemException, titulo);

        [NonAction]
        protected void ExibirMensagemInformacao(string mensagem, string titulo = "Informação!") => ExibirMensagem(ETipoMensagem.Informacao, mensagem, titulo);

        [NonAction]
        protected void ExibirMensagemSucesso(string mensagem, string titulo = "Sucesso!") => ExibirMensagem(ETipoMensagem.Sucesso, mensagem, titulo);

        [NonAction]
        protected void ExibirMensagemAlerta(string mensagem, string titulo = "Atenção!") => ExibirMensagem(ETipoMensagem.Perigo, mensagem, titulo);

        [NonAction]
        protected void ExibirMensagem(ETipoMensagem tipo, string mensagem, string titulo = null)
        {
            var m = new Framework.UI.Mensagem.Mensagem(tipo, titulo, mensagem);
            m.Salvar(TempData);
        }

        [NonAction]
        protected void ExibirMensagem(MensagemException mensagemException, string titulo = null)
        {
            var m = new Framework.UI.Mensagem.Mensagem(ETipoMensagem.Erro, titulo, mensagemException.GetMessages("<br />"));
            m.Salvar(TempData);
        }

        #endregion

        protected Task<AuthUsuario<TUsuarioIdentificao>> AutenticarUsuarioAsync(TUsuarioIdentificao usuarioIdentificao, bool lembrarMe = false) =>
            HttpContext.AutenticarAsync(usuarioIdentificao, lembrarMe);

        protected Task DesautenticarUsuarioAsync() => HttpContext.DesautenticarAsync();
    }
}

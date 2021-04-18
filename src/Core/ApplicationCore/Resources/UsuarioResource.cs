using Framework.Resources;
using Microsoft.Extensions.Localization;

namespace ApplicationCore.Resources
{
    public class UsuarioResource : ResourceBase
    {
        public virtual LocalizedString NaoLocalizado => Resource("Erro001", "O usuário não foi localizado.");
        public virtual LocalizedString Inativo => Resource("Erro002", "O usuário está desativado.");
        public virtual LocalizedString SenhaErrada => Resource("Erro003", "Não foi possível autenticar o usuário, verifique o e-mail e/ou senha.");
        public virtual LocalizedString UsuarioJaPossuiCadastro => Resource("Erro004", "Não é possível efetuar o cadastro, já existe um usuário cadastrado para o endereço de e-mail.");
        public virtual LocalizedString SenhaEConfirmacaoNaoConferem => Resource("Erro005", "Sua nova senha não confere com a confirmação de senha.");
        public virtual LocalizedString SenhaErradaAlteracao => Resource("Erro006", "Não foi possível alterar sua senha, verifique o e-mail e/ou senha fornecidos!");
    }
}

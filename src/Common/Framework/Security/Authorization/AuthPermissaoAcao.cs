using System;

namespace Framework.Security.Authorization
{
    public class AuthPermissaoAcao
    {
        public AuthPermissaoAcao()
        {}

        public AuthPermissaoAcao(Guid id, AuthPermissao funcionalidade, AuthPermissaoTipoAcao acao)
        {
            Id = id;
            Funcionalidade = funcionalidade;
            Acao = acao;
        }

        public Guid Id { get; set; }
        public AuthPermissao Funcionalidade { get; set; }
        public AuthPermissaoTipoAcao Acao { get; set; }
    }
}
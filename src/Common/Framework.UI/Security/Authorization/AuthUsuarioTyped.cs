using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Framework.UI.Security.Authorization
{
    [Serializable]
    public class AuthUsuario<TIdentificacao> : AuthUsuario where TIdentificacao : IAuthUsuarioIdentificacao
    {

        public TIdentificacao Identificao { get; set; }

        internal AuthUsuario()
            : base()
        { }

        internal void FillClaims(List<Claim> claim)
        {
            Identificao.FillClaims(claim);
        }

        internal AuthUsuario(TIdentificacao identificao)
            : base(true)
        {
            Identificao = identificao;
        }


        internal AuthUsuario(IEnumerable<Claim> claims)
            : base(true)
        {
            Identificao = Activator.CreateInstance<TIdentificacao>();
            Identificao.Bind(claims);
        }
    }
}

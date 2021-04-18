using System.Collections.Generic;
using System.Security.Claims;

namespace Framework.UI.Security.Authorization
{
    public interface IAuthUsuarioIdentificacao
    {
        void FillClaims(List<Claim> claim);
        void Bind(IEnumerable<Claim> claim);
    }

}

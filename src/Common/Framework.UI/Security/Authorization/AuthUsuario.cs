using Framework.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.UI.Security.Authorization
{
    [Serializable]
    public class AuthUsuario
    {
        public bool Autenticado { get; set; }

        public List<Guid> PermissoesAcoes { get; } = new List<Guid>();

        internal AuthUsuario() => Autenticado = false;

        internal AuthUsuario(bool autenticado)
        {
            Autenticado = autenticado;
        }

        public bool ValidarAcesso(string idFuncionalidade, AuthPermissaoTipoAcao acao)
        {
            if (!Guid.TryParse(idFuncionalidade, out var idFunc))
                throw new Exception($"O ID Funcionalidade {idFuncionalidade} não é válido");

            return ValidarAcesso(idFunc, acao);
        }

        public bool ValidarAcessoGrupo(string[] idFuncionalidade)
        {
            foreach (var item in idFuncionalidade)
            {
                if (!Guid.TryParse(item, out var idFunc))
                    throw new Exception($"O ID Funcionalidade {idFuncionalidade} não é válido");

                if (ValidarAcesso(idFunc))
                    return true;
            }

            return false;
        }

        public bool ValidarAcesso(Guid idPermissao, AuthPermissaoTipoAcao acao)
        {
            var funcionalidade = AuthPermissao.Todas.FirstOrDefault(f => f.Id == idPermissao);
            if (funcionalidade == null)
                throw new Exception("Permissão não registrada");

            var funcAcao = funcionalidade.Acoes.FirstOrDefault(a => a.Acao == acao);
            if (funcAcao == null)
                throw new Exception("Ação não registrada para a permissão");


            return PermissoesAcoes.Contains(funcAcao.Id);
        }

        public bool ValidarAcesso(Guid idPermissao)
        {
            var funcionalidade = AuthPermissao.Todas.FirstOrDefault(f => f.Id == idPermissao);
            if (funcionalidade == null)
                throw new Exception("Permissão não registrada");

            var funcAcao = funcionalidade.Acoes.FirstOrDefault(a => a.Acao == AuthPermissaoTipoAcao.Permitir);
            if (funcAcao == null)
                throw new Exception("Ação não registrada para a permissão");


            return PermissoesAcoes.Contains(funcAcao.Id);
        }

        public virtual void FillPermissoesAcoes(IEnumerable<Guid> permissoesAcoes)
        {
            if (permissoesAcoes != null)
                PermissoesAcoes.AddRange(permissoesAcoes);
        }
    }
}

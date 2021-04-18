using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class DashboardPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{843EAFF1-632C-4FC8-916C-41CA3810EF1A}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{A0AC661E-809F-4C47-8204-7891A8F8BA9E}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Dashboard";
        public static string Descricao => "Visualizar Dashboard";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Leitura.Key, Leitura.Value);
        }
    }
}

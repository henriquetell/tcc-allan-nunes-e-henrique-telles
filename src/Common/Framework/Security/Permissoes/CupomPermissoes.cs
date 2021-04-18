using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class CupomPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{3A018FDF-FC5E-49AE-87D0-401698F8561B}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Escrever => new KeyValuePair<string, AuthPermissaoTipoAcao>("{09CADA9E-7AC3-4B7E-90ED-ACDF03D8247D}", AuthPermissaoTipoAcao.Escrever);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{78BB0A3E-032E-47EF-BDFA-7A1A4C8C6F67}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Cupom";
        public static string Descricao => "Gerenciar Cupons";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Escrever.Key, Escrever.Value)
                .AddAcao(Leitura.Key, Leitura.Value);
        }
    }
}

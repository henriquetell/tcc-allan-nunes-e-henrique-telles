using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class RelatorioPedidosPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{445D4DDC-D572-480B-9F1E-28B91AEF41C6}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{C1A97A95-BDB6-4A5B-9634-46FBBE780DBD}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Relatório de Pedidos";
        public static string Descricao => "Emissão de Relatório de Pedidos";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Leitura.Key, Leitura.Value);
        }
    }
}

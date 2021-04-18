using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class PedidoPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{66D18999-08DA-4994-9856-D15F5BD141BF}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Escrever => new KeyValuePair<string, AuthPermissaoTipoAcao>("{DB766FFF-4773-4E04-89BC-BB6B7FFB56C9}", AuthPermissaoTipoAcao.Escrever);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Excluir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{5A680DC9-657B-4820-8DC8-4C2A43D35167}", AuthPermissaoTipoAcao.Excluir);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{A5FE8CA8-4452-4A91-AEE5-20DFDD0F6010}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Pedido";
        public static string Descricao => "Gerenciar Pedidos";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Escrever.Key, Escrever.Value)
                .AddAcao(Leitura.Key, Leitura.Value)
                .AddAcao(Excluir.Key, Excluir.Value);
        }
    }
}

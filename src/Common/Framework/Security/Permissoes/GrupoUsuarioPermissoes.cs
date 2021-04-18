using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class GrupoUsuarioPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{8D047FA4-3F15-43D8-9B53-B2B5F9464232}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Escrever => new KeyValuePair<string, AuthPermissaoTipoAcao>("{482E569B-D32B-4891-BA21-C18398FBB6B4}", AuthPermissaoTipoAcao.Escrever);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Excluir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{2A49A522-1B92-4374-BB14-1CEB123CB08D}", AuthPermissaoTipoAcao.Excluir);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{DC830BDA-9991-4B44-8BE5-A60F99D1E90B}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Grupo de Acesso";
        public static string Descricao => "Gerenciar Grupo de Acesso";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Escrever.Key, Escrever.Value)
                .AddAcao(Leitura.Key, Leitura.Value)
                .AddAcao(Excluir.Key, Excluir.Value);
        }
    }
}

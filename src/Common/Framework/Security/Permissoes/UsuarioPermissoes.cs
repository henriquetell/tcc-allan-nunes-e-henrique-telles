using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class UsuarioPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{BC158B92-D6BD-4FBB-BAFF-4B4303AF7F19}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Escrever => new KeyValuePair<string, AuthPermissaoTipoAcao>("{D3284D17-62E1-4E95-AA1B-B0ED556CF72D}", AuthPermissaoTipoAcao.Escrever);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Excluir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{49651B71-2979-4744-8843-C280B6C9D503}", AuthPermissaoTipoAcao.Excluir);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{33B05B6E-3992-4B52-A036-AF90D719DDBC}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Usuários";
        public static string Descricao => "Gerenciar Usuários";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Escrever.Key, Escrever.Value)
                .AddAcao(Leitura.Key, Leitura.Value)
                .AddAcao(Excluir.Key, Excluir.Value);
        }
    }
}

using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class ConteudoPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{7F5D3EFD-2C3D-44B3-B371-A442F8E5595C}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Escrever => new KeyValuePair<string, AuthPermissaoTipoAcao>("{9C6EE102-FC50-4D97-90DC-7755C67125D4}", AuthPermissaoTipoAcao.Escrever);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Excluir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{1D6ABA9D-8456-4A4F-999C-AA8B3DCEE70B}", AuthPermissaoTipoAcao.Excluir);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{16505584-AC70-441B-9317-B8555AC8CF62}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Conteúdo";
        public static string Descricao => "Gerenciar Conteúdos";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Escrever.Key, Escrever.Value)
                .AddAcao(Leitura.Key, Leitura.Value)
                .AddAcao(Excluir.Key, Excluir.Value);
        }
    }
}

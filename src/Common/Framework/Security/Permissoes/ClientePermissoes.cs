using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class ClientePermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{135F951D-2867-4C64-A77B-CA461C1D9D5B}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Escrever => new KeyValuePair<string, AuthPermissaoTipoAcao>("{9EA6C92A-D884-466A-9C18-58341D6AA7F0}", AuthPermissaoTipoAcao.Escrever);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Excluir => new KeyValuePair<string, AuthPermissaoTipoAcao>("{94B540FF-5B62-4BD5-9E6F-EFDD5082B7F4}", AuthPermissaoTipoAcao.Excluir);
        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{BC0987B6-DDB0-4DA6-8663-7E52DF52EFE0}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Clientes";
        public static string Descricao => "Gerenciar Clientes";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Escrever.Key, Escrever.Value)
                .AddAcao(Leitura.Key, Leitura.Value)
                .AddAcao(Excluir.Key, Excluir.Value);
        }
    }
}

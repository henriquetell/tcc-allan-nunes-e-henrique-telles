using Framework.Security.Authorization;
using System.Collections.Generic;

namespace Framework.Security.Permissoes
{
    public class RelatorioFaturamentoPermissoes : RegistradorAuthPermissao
    {
        public const string Gerenciar = "{AFA15902-2CD2-4257-9FB2-A34A6CB6A396}";

        public static KeyValuePair<string, AuthPermissaoTipoAcao> Leitura => new KeyValuePair<string, AuthPermissaoTipoAcao>("{949F24A5-17FB-4A8C-8425-3EE854DBDC87}", AuthPermissaoTipoAcao.Leitura);

        protected override string Grupo => NomeGrupo;
        public static string NomeGrupo => "Relatório de Faturamento";
        public static string Descricao => "Emissão de Relatório de Faturamento";

        public override void Init()
        {
            Registrar(Gerenciar, Descricao)
                .AddAcao(Leitura.Key, Leitura.Value);
        }
    }
}

using Framework.Extenders;
using Framework.ValueObjects;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ApplicationCore.Interfaces.CloudServices.CloudQueue
{
    public partial class CloudQueueNames
    {
        public const string EnviarEmailKey = "%enviar-email%";
        public static readonly CloudQueueNames EnviarEmail = new CloudQueueNames(EnviarEmailKey, "Envio de e-mail");

        public const string EnviarEmailConfirmacaoKey = "%enviar-email-confirmacao%";
        public static readonly CloudQueueNames EnviarEmailConfirmacao = new CloudQueueNames(EnviarEmailConfirmacaoKey, "Envio de e-mail de confirmação de pedido");

        public const string EnviarEmailPagamentoKey = "%enviar-email-pagamento%";
        public static readonly CloudQueueNames EnviarEmailPagamento = new CloudQueueNames(EnviarEmailPagamentoKey, "Envio de e-mail de pagamento de pedido");

        public const string EnviarEmailEstornoKey = "%enviar-email-estorno%";
        public static readonly CloudQueueNames EnviarEmailEstorno= new CloudQueueNames(EnviarEmailEstornoKey, "Envio de e-mail de estorno");

        public const string EnviarEmailCancelamentoKey = "%enviar-email-cancelamento%";
        public static readonly CloudQueueNames EnviarEmailCancelamento = new CloudQueueNames(EnviarEmailCancelamentoKey, "Envio de e-mail de cancelamento");
    }

    [Serializable]
    public partial class CloudQueueNames : Enumeration<CloudQueueNames, string>
    {
        public readonly string NomeReal;

        public string NomeFila => Value;
        public string Descricao => DisplayName;


        public CloudQueueNames(string nomeFila, string descricaoFila)
            : base(nomeFila, descricaoFila)
        {
            NomeReal = CriarNomeReal(nomeFila);
        }

        public static implicit operator CloudQueueNames(string nomeFila)
        {
            return GetAll().FirstOrDefault(c => c.Value == nomeFila);
        }

        public static string CriarNomeReal(string nomeFila)
        {
            nomeFila = nomeFila.Trim('%');

            if (EnvironmentHelper.Desenvolvimento)
            {
                var machineName = Regex.Replace(Environment.MachineName.ToLower(), "[^a-z0-9]", "");
                return $"dev-{machineName}-{nomeFila}";
            }

            return nomeFila;
        }

    }
}

using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Interfaces.Email
{
    public class DadosEnvioEmail
    {
        public List<Framework.ValueObjects.Email> Destinatario { get; set; }

        public string Assunto { get; set; }
        public string Corpo { get; set; }

        public int? UsarConfiguracaoIdParceiro { get; set; }

        private DadosEnvioEmail() { }

        public DadosEnvioEmail(IEnumerable<Framework.ValueObjects.Email> destinatario, string assunto, string corpo)
        {
            Destinatario = destinatario.ToList();
            Assunto = assunto;
            Corpo = corpo;
        }

        public DadosEnvioEmail(IEnumerable<string> destinatario, string assunto, string corpo)
            :this(destinatario.Select(d => (Framework.ValueObjects.Email)d), assunto, corpo)
        {
        }

        public DadosEnvioEmail(Framework.ValueObjects.Email destinatario, string assunto, string corpo)
            : this(new[] { destinatario }, assunto, corpo)
        {
        }

    }
}

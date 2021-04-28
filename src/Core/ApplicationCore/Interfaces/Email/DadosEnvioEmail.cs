using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Interfaces.Email
{
    public class DadosEnvioEmail
    {
        public List<string> Destinatario { get; set; }

        public string Assunto { get; set; }
        public string Corpo { get; set; }

        public int? UsarConfiguracaoIdParceiro { get; set; }

        private DadosEnvioEmail() { }

        public DadosEnvioEmail(IEnumerable<string> destinatario, string assunto, string corpo)
        {
            Destinatario = destinatario.ToList();
            Assunto = assunto;
            Corpo = corpo;
        }


        public DadosEnvioEmail(string destinatario, string assunto, string corpo)
            : this(new[] { destinatario }, assunto, corpo)
        {
        }

    }
}

using ApplicationCore.Enuns;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Respositories;
using Framework.Extenders;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class EmailService : ServiceBase
    {
        private IConteudoRepository ConteudoRepository => GetService<IConteudoRepository>();
        private IEmailClient EmailClient => GetService<IEmailClient>();


        public EmailService(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        public async Task EnviarFaleConosco(string nome, string email, string telefone, string mensagem)
        {
            var corpo = $@"Você recebeu uma mensagem através do Fale Conosco!<br>
                           Nome: {nome}<br>
                           E-mail: {email}<br>
                           Telefone: {telefone}<br>
                           Mensagem: {mensagem}";

            await EmailClient.EnviarParaFilaAsync(new DadosEnvioEmail(AppConfig.DestinatarioFaleConosco, $"{AppConfig.Projeto} | Fale Conosco: {nome}", corpo));
        }
    }
}

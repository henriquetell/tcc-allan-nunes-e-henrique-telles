using ApplicationCore.Interfaces.Email;
using Framework.Extenders;
using Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EmailClient : IEmailClient
    {
        private readonly IServiceProvider _serviceProvider;

        private InfrastructureConfig InfrastructureConfig => _serviceProvider.GetService<InfrastructureConfig>();

        public EmailClient(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task EnviarAsync(DadosEnvioEmail dadosEmail)
        {
            using var smtp = new SmtpClient();
            using var mensagem = new MailMessage();

            BindSmtpClient(smtp);

            mensagem.Subject = dadosEmail.Assunto;
            mensagem.Body = dadosEmail.Corpo;
            mensagem.IsBodyHtml = true;

            foreach (var item in dadosEmail.Destinatario)
                mensagem.To.Add(item);

            BindMailMessage(mensagem);

            await smtp.SendMailAsync(mensagem);
        }

        private void BindSmtpClient(SmtpClient smtpClient)
        {
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Host = InfrastructureConfig.Email.Servidor;
            smtpClient.Port = InfrastructureConfig.Email.Porta;
            smtpClient.EnableSsl = InfrastructureConfig.Email.Ssl;
            smtpClient.Credentials = new NetworkCredential(InfrastructureConfig.Email.Usuario, InfrastructureConfig.Email.Senha);
        }

        private void BindMailMessage(MailMessage mensagem)
        {
            mensagem.From = new MailAddress(InfrastructureConfig.Email.EnderecoRemetente, InfrastructureConfig.Email.NomeRemetente);

            if ((EnvironmentHelper.Desenvolvimento || EnvironmentHelper.Homologacao) &&
                !string.IsNullOrWhiteSpace(InfrastructureConfig.Email.DebugEmail))
            {
                mensagem.To.Clear();

                foreach (var e in InfrastructureConfig.Email.DebugEmail.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    mensagem.To.Add(e);
            }
        }
    }
}

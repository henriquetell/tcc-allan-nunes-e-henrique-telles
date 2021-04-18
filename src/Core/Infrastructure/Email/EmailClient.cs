using ApplicationCore.DataValue.Conteudo;
using ApplicationCore.Enuns;
using ApplicationCore.Extenders;
using ApplicationCore.Interfaces.CloudServices.CloudQueue;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Interfaces.Logging;
using Framework.Extenders;
using Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EmailClient : IEmailClient
    {
        private readonly IServiceProvider _serviceProvider;

        private IAppLogger<EmailClient> AppLogger => _serviceProvider.GetService<IAppLogger<EmailClient>>();
        private ICloudQueueService CloudQueueService => _serviceProvider.GetService<ICloudQueueService>();
        private InfrastructureConfig InfrastructureConfig => _serviceProvider.GetService<InfrastructureConfig>();

        public EmailClient(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task EnviarParaFilaConfirmacaoAsync(int idPedido)
        {
            var envioTimer = AppLogger.Info($"Adicionando na fila o e-mail os e-mails do pedido(Confirmação e Pagamento)").StartTimer();
            await CloudQueueService.SendAsync(idPedido, CloudQueueNames.EnviarEmailConfirmacao);
            envioTimer.Stop((t, l) => l.Info($"Tempo para adicionar o e-mail na fila: {TimeSpan.FromMilliseconds(t)}"));
        }

        public async Task EnviarParaFilaPagamentoAsync(int idPedido)
        {
            var envioTimer = AppLogger.Info($"Adicionando na fila o e-mail os e-mails do pedido(Confirmação e Pagamento)").StartTimer();
            await CloudQueueService.SendAsync(idPedido, CloudQueueNames.EnviarEmailPagamento);
            envioTimer.Stop((t, l) => l.Info($"Tempo para adicionar o e-mail na fila: {TimeSpan.FromMilliseconds(t)}"));
        }


        public async Task EnviarParaFilaCancelamentoAsync(int idPedido)
        {
            var envioTimer = AppLogger.Info($"Adicionando na fila o e-mail de cancelamento").StartTimer();
            await CloudQueueService.SendAsync(idPedido, CloudQueueNames.EnviarEmailCancelamento);
            envioTimer.Stop((t, l) => l.Info($"Tempo para adicionar o e-mail na fila: {TimeSpan.FromMilliseconds(t)}"));
        }

        public async Task EnviarParaFilaEstornoAsync(int idPedido)
        {
            var envioTimer = AppLogger.Info($"Adicionando na fila o e-mail de estorno").StartTimer();
            await CloudQueueService.SendAsync(idPedido, CloudQueueNames.EnviarEmailEstorno);
            envioTimer.Stop((t, l) => l.Info($"Tempo para adicionar o e-mail na fila: {TimeSpan.FromMilliseconds(t)}"));
        }

        public async Task EnviarParaFilaAsync(DadosEnvioEmail dadosEmail)
        {
            if (dadosEmail.Destinatario.Any(d => !d.Valido))
            {
                var invalidos = dadosEmail.Destinatario.Where(d => !d.Valido);
                var ex = new InvalidOperationException($"O e-mail \"{dadosEmail.Assunto}\" não será enviado para \"{string.Join(",", invalidos.Select(e => e.Value))}\" porque contem um endereço de e-mail inválido");
                AppLogger.Exception(ex);
                throw ex;
            }

            var envioTimer = AppLogger.Info($"Adicionando na fila o e-mail \"{dadosEmail.Assunto}\" para \"{string.Join(",", dadosEmail.Destinatario.Select(e => e.Value))}\"").StartTimer();

            await CloudQueueService.SendAsync(dadosEmail, CloudQueueNames.EnviarEmail);

            envioTimer.Stop((t, l) => l.Info($"Tempo para adicionar o e-mail na fila: {TimeSpan.FromMilliseconds(t)}"));
        }

        public async Task EnviarAsync(DadosEnvioEmail dadosEmail)
        {
            AppLogger.Info($"Enviando e-mail \"{dadosEmail.Assunto}\" para \"{string.Join(";", dadosEmail.Destinatario.Select(c => c.Value))}\"");

            using (var smtp = new SmtpClient())
            {
                BindSmtpClient(smtp, dadosEmail);

                using (var mensagem = new MailMessage())
                {
                    mensagem.Subject = dadosEmail.Assunto;
                    mensagem.Body = dadosEmail.Corpo;
                    mensagem.IsBodyHtml = true;

                    foreach (var item in dadosEmail.Destinatario)
                        mensagem.To.Add(item.Value);

                    BindMailMessage(mensagem, dadosEmail);

                    try
                    {
                        var envioTimer = AppLogger.StartTimer();
                        await smtp.SendMailAsync(mensagem);
                        envioTimer.Stop((t, l) => l.Info($"Tempo para envio de e-mail: {TimeSpan.FromMilliseconds(t)}"));

                    }
                    catch (Exception ex)
                    {
                        AppLogger.Exception(ex, "Erro ao enviar o e-mail");
                        throw;
                    }
                }
            }
        }

        public async Task EnviarComAnexoAsync(DadosEnvioEmail dadosEmail, (Stream content, string name) anexo)
        {
            AppLogger.Info($"Enviando e-mail \"{dadosEmail.Assunto}\" para \"{string.Join(";", dadosEmail.Destinatario.Select(c => c.Value))}\"");

            using (var smtp = new SmtpClient())
            {
                BindSmtpClient(smtp, dadosEmail);

                using (var mensagem = new MailMessage())
                {
                    if (anexo.content != null)
                    {
                        anexo.content.Seek(0, SeekOrigin.Begin);
                        mensagem.Attachments.Add(new Attachment(anexo.content, anexo.name));
                    }

                    mensagem.Subject = EnvironmentHelper.Producao
                        ? dadosEmail.Assunto
                        : $"[DEV] {dadosEmail.Assunto}";

                    mensagem.Body = dadosEmail.Corpo;
                    mensagem.IsBodyHtml = true;

                    foreach (var item in dadosEmail.Destinatario)
                        mensagem.To.Add(item.Value);

                    BindMailMessage(mensagem, dadosEmail);

                    try
                    {
                        var envioTimer = AppLogger.StartTimer();
                        await smtp.SendMailAsync(mensagem);
                        envioTimer.Stop((t, l) => l.Info($"Tempo para envio de e-mail: {TimeSpan.FromMilliseconds(t)}"));

                    }
                    catch (Exception ex)
                    {
                        AppLogger.Exception(ex, "Erro ao enviar o e-mail");
                        throw;
                    }
                    finally
                    {
                        foreach (var item in mensagem.Attachments)
                        {
                            item.ContentStream?.Dispose();
                            item?.Dispose();
                        }
                    }
                }
            }
        }

        private void BindSmtpClient(SmtpClient smtpClient, DadosEnvioEmail dadosEmail)
        {
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;


            AppLogger.Info($"Usando configuração local");
            smtpClient.Host = InfrastructureConfig.Email.Servidor;
            smtpClient.Port = InfrastructureConfig.Email.Porta;
            smtpClient.EnableSsl = InfrastructureConfig.Email.Ssl;
            smtpClient.Credentials = new NetworkCredential(InfrastructureConfig.Email.Usuario, InfrastructureConfig.Email.Senha);
        }

        private void BindMailMessage(MailMessage mensagem, DadosEnvioEmail dadosEmail)
        {
            mensagem.From = new MailAddress(InfrastructureConfig.Email.EnderecoRemetente, InfrastructureConfig.Email.NomeRemetente);

            if ((EnvironmentHelper.Desenvolvimento || EnvironmentHelper.Homologacao) &&
                !string.IsNullOrWhiteSpace(InfrastructureConfig.Email.DebugEmail))
            {
                AppLogger.Info($"Usando configuração de debug, o e-email será enviado para: {InfrastructureConfig.Email.DebugEmail}");
                mensagem.To.Clear();

                foreach (var e in InfrastructureConfig.Email.DebugEmail.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    mensagem.To.Add(e);
            }
        }
    }
}

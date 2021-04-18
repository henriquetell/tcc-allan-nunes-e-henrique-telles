using ApplicationCore.DataValue.Conteudo;
using System;

namespace ApplicationCore.Interfaces.Email.Eventos
{
    public abstract class EmailEventInvoker
    {
        public abstract void OnSucesso(DadosEnvioEmailConteudo dadosEnvioEmailConteudo);
    }


    public class EmailEventInvoker<TEvent> : EmailEventInvoker where TEvent : IEmailEvent
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailEventInvoker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override void OnSucesso(DadosEnvioEmailConteudo dadosEnvioEmailConteudo)
        {
            var evento = Activator.CreateInstance<TEvent>();
            evento.ServiceProvider = _serviceProvider;
            evento.DadosEnvioEmailConteudo = dadosEnvioEmailConteudo;
            evento.OnSucesso();
        }
    }
}

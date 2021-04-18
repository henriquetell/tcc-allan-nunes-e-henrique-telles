using ApplicationCore.DataValue.Conteudo;
using System;

namespace ApplicationCore.Interfaces.Email.Eventos
{
    public interface IEmailEvent
    {
        DadosEnvioEmailConteudo DadosEnvioEmailConteudo { get; set; }
        IServiceProvider ServiceProvider { get; set; }

        void OnSucesso();
    }
}

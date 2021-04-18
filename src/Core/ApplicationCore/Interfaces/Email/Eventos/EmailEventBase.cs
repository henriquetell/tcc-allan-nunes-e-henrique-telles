using ApplicationCore.DataValue.Conteudo;
using ApplicationCore.Respositories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApplicationCore.Interfaces.Email.Eventos
{
    internal abstract class EmailEventBase : IEmailEvent
    {
        public DadosEnvioEmailConteudo DadosEnvioEmailConteudo { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        protected TService GetService<TService>()
        {
            if (typeof(IRepository).IsAssignableFrom(typeof(TService)))
            {
                return (TService)ServiceProvider.GetService<RepositoryFactory>().Create(typeof(TService));
            }

            return ServiceProvider.GetService<TService>();

        }

        protected TEmailDataValue ConteudoDataSource<TEmailDataValue>() where TEmailDataValue : class, IEmailDataValue =>
            DadosEnvioEmailConteudo.RecuperarConteudoDataSource<TEmailDataValue>() as TEmailDataValue;


        public abstract void OnSucesso();
    }
}

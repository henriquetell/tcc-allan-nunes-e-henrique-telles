using ApplicationCore.Configurations;
using ApplicationCore.DependencyInjection;
using Infrastructure.Configurations;
using Infrastructure.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(NpsFunctions.Startup))]
namespace NpsFunctions
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<InfrastructureConfig>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.GetSection("Infrastructure").Bind(settings);
                            });

            builder.Services.AddOptions<ApplicationCoreConfig>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.GetSection("ApplicationCore").Bind(settings);
                            });

            using var serviceProvider = builder.Services.BuildServiceProvider();
            var infraConfig = serviceProvider.GetService<IOptions<InfrastructureConfig>>().Value;

            builder.Services
                    .AddCore(sp => sp.GetService<IOptions<ApplicationCoreConfig>>().Value)
                    .AddInfrastructure(sp => sp.GetService<IOptions<InfrastructureConfig>>().Value)
                    .AddDbContext(infraConfig.DefaultConnection);
        }
    }
}

using ApplicationCore.Services;
using Framework.Security.Authorization;
using Infrastructure.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                scope.ServiceProvider.RunMigration();
                RegistrarAuthFuncionalidade();
                scope.ServiceProvider.GetService<FuncionalidadeService>()
                    .Sincronizar(AuthPermissao.Todas);
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
                .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                .ConfigureLogging((context, l) =>
                {
                    l.AddConfiguration(context.Configuration.GetSection("Logging"));
                    l.ClearProviders();
#if DEBUG
                    l.AddConsole();
#endif
                    l.SetMinimumLevel(LogLevel.Warning);
                }).Build();

        private static void RegistrarAuthFuncionalidade()
        {
            var registers = typeof(RegistradorAuthPermissao).Assembly.GetTypes()
                .Where(t => typeof(RegistradorAuthPermissao).IsAssignableFrom(t) &&
                            typeof(RegistradorAuthPermissao) != t)
                .Select(register => Activator.CreateInstance(register) as RegistradorAuthPermissao);

            foreach (var r in registers)
                r?.Init();
        }
    }
}

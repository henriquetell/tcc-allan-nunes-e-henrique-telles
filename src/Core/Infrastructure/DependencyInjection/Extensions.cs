using ApplicationCore.Extenders;
using ApplicationCore.Interfaces.CloudServices.CloudQueue;
using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Interfaces.Logging;
using ApplicationCore.Respositories;
using CNA.BemMaisAgro.Infrastructure.Logging;
using Framework.Extenders;
using Infrastructure.CloudServices;
using Infrastructure.Configurations;
using Infrastructure.Email;
using Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DependencyInjection
{
    public static class Extensions
    {
        private const string ConfigSectionName = nameof(Infrastructure);

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            Func<IServiceProvider, InfrastructureConfig> infrastructureConfigFactory,
            Type appLoggerType = null,
            ILoggerProvider efContextLoggerProvider = null,
            bool healthCheck = false)
        {

            var repositoriesImplemented = GetRepositoriesImplemented();

            services = services
                .AddSingleton(infrastructureConfigFactory)
                .AddInfrastructureRepositories(repositoriesImplemented)
                .AddReadOnlyRepositories(repositoriesImplemented)
                .AddCudOperationFactory()
                         .AddAppLogger()
                .AddScoped<IEmailClient, EmailClient>()
                .AddScoped<ICloudQueueService, AzureQueue>()
                .AddScoped<ICloudStorage, AzureStorage>();

            return services;
        }

        private static List<Type> GetRepositoriesImplemented()
        {
            var implementationsType = typeof(Extensions).Assembly.GetTypes()
                .Where(t => typeof(RespositoryBase).IsAssignableFrom(t) &&
                       !t.IsGenericType &&
                       t.BaseType != typeof(object)
                      );

            return implementationsType.ToList();
        }

        private static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services, IEnumerable<Type> repositoriesImplemented)
        {
            foreach (var implementationType in repositoriesImplemented)
            {
                services.AddScoped(implementationType);
            }

            return services;
        }

        private static IServiceCollection AddReadOnlyRepositories(this IServiceCollection services, IEnumerable<Type> repositoriesImplemented)
        {
            foreach (var implementationType in repositoriesImplemented)
            {
                var repositoryInterface = implementationType.GetInterfaces()
                    .FirstOrDefault(i => i.GetInterface(typeof(IReadOnlyRepository<,>).FullName) != null &&
                                         i.GetInterface(typeof(IRepository<,>).FullName) == null &&
                                         i.Name != typeof(IReadOnlyRepository<>).Name);

                services.AddScoped(repositoryInterface, sp => sp.GetService(implementationType));
            }

            return services;
        }

        private static IServiceCollection AddCudOperationFactory(this IServiceCollection services) =>
            services.AddScoped<RepositoryFactory, DefaultCudOperationFactory>();

        private static IServiceCollection AddAppLogger(this IServiceCollection services) =>
            services.AddTransient<IAppLoggerFactory, AppLoggerFactory>()
                    .AddTransient(typeof(IAppLogger<>), typeof(BasicAppLogger<>));

        public static IServiceCollection AddEntityFramework(this IServiceCollection services, ILoggerProvider efContextLoggerProvider)
        {
            LoggerFactory loggerFactory = null;
            if (efContextLoggerProvider != null)
            {
                loggerFactory = new LoggerFactory();
                loggerFactory.AddProvider(efContextLoggerProvider);
            }

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<EfContext>((sp, b) =>
                    b.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .EnableSensitiveDataLogging(EnvironmentHelper.Desenvolvimento)
                        .UseSqlServer(sp.GetService<InfrastructureConfig>().ConnectionString)
                        .UseLoggerFactory(loggerFactory)
                );

            return services;
        }

        public static IServiceProvider RunMigration(this IServiceProvider service)
        {
            using (var serviceScope = service.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var dbContext = serviceScope.ServiceProvider.GetService<EfContext>())
            {
                var appLogger = serviceScope.ServiceProvider.AppLogger<EfContext>();

                dbContext.Database.SetCommandTimeout(180);

                try
                {
                    var migrationTimer = appLogger.Info("Aplicando migration...").StartTimer();
                    dbContext.Database.Migrate();
                    migrationTimer.Stop((t, l) => l.Info($"Migration aplicado com sucesso. Tempo de execução {TimeSpan.FromMilliseconds(t)}"));

                    var seedTimer = appLogger.Info("Aplicando seed...").StartTimer();
                    dbContext.Seed(appLogger);
                    seedTimer.Stop((t, l) => l.Info($"Seed aplicado com sucesso. Tempo de execução {TimeSpan.FromMilliseconds(t)}"));
                }
                catch (Exception ex)
                {
                    ex = new Exception("Ocorreu ao executar o migration", ex);
                    appLogger?.Exception(ex);

                    throw;
                }
            }

            return service;
        }
    }
}

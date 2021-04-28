using ApplicationCore.Interfaces.CloudServices.CloudQueue;
using ApplicationCore.Interfaces.CloudServices.CloudStorage;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Respositories;
using Infrastructure.CloudServices;
using Infrastructure.Configurations;
using Infrastructure.Email;
using Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DependencyInjection
{
    public static class Extensions
    {
        private const string ConfigSectionName = nameof(Infrastructure);

        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            Func<IServiceProvider, InfrastructureConfig> infrastructureConfigFactory)
        {

            var repositoriesImplemented = GetRepositoriesImplemented();

            services = services
                .AddSingleton(infrastructureConfigFactory)
                .AddInfrastructureRepositories(repositoriesImplemented)
                .AddReadOnlyRepositories(repositoriesImplemented)
                .AddCudOperationFactory()
                .AddScoped<IEmailClient, EmailClient>()
                .AddScoped<ICloudQueueService, AzureQueue>()
                .AddScoped<ICloudStorage, AzureStorage>();

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<EfContext>(options => options.UseSqlServer(connectionString));
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

        public static IServiceProvider RunMigration(this IServiceProvider service)
        {
            using (var serviceScope = service.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var dbContext = serviceScope.ServiceProvider.GetService<EfContext>())
            {

                dbContext.Database.SetCommandTimeout(180);
                dbContext.Database.Migrate();
                dbContext.Seed();
            }

            return service;
        }
    }
}

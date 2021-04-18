using ApplicationCore.Interfaces.CloudServices.CloudQueue;
using ApplicationCore.Interfaces.Logging;
using Azure.Storage.Queues;
using Infrastructure.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CloudServices
{
    public class AzureQueue : ICloudQueueService
    {
        private readonly IAppLogger<AzureQueue> _appLogger;
        private readonly InfrastructureConfig _infrastructureConfig;
        private readonly IMemoryCache _cache;

        public AzureQueue(IMemoryCache cache, InfrastructureConfig config, IAppLogger<AzureQueue> appLogger)
        {
            _cache = cache;
            _infrastructureConfig = config;
            _appLogger = appLogger;
        }

        public async Task SendAsync(object item, CloudQueueNames queueName, TimeSpan? initialVisibilityDelay = null)
        {
            var queueClient = await CreateCloudQueueAsync(queueName);
            var queueMessage = await queueClient.SendMessageAsync(EncodeMessage(item), initialVisibilityDelay);

            _appLogger.Info($"A Mensagem {queueMessage.Value.MessageId} foi adicionado a fila {queueName.NomeFila}");
        }

        private static string EncodeMessage(object item)
        {
            var jsonMessage = JsonConvert.SerializeObject(item);

            var inputAsBytes = Encoding.UTF8.GetBytes(jsonMessage);
            return Convert.ToBase64String(inputAsBytes);
        }

        private async Task<QueueClient> CreateCloudQueueAsync(CloudQueueNames queueName)
        {
            var cacheKey = $"CloudQueue|{queueName.NomeFila}";
            Exception cacheEx = null;
            var queue = await _cache.GetOrCreateAsync(cacheKey, e => Task.Run(async delegate
            {
                try
                {
                    e.SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    var queueClient = new QueueClient(_infrastructureConfig.Storage.ConnectionString, queueName.NomeFila);
                    await queueClient.CreateIfNotExistsAsync();

                    return queueClient;
                }
                catch (Exception ex)
                {
                    cacheEx = ex;
                }

                return null;
            }));

            if (cacheEx != null || queue == null)
            {
                _cache.Remove(cacheKey);

                if (cacheEx != null)
                    throw cacheEx;
            }

            return queue;
        }
    }
}

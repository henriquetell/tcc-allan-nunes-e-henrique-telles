using ApplicationCore.Interfaces.CloudServices.CloudQueue;
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
        private readonly InfrastructureConfig _infrastructureConfig;
        private readonly IMemoryCache _cache;

        public AzureQueue(IMemoryCache cache, InfrastructureConfig config)
        {
            _cache = cache;
            _infrastructureConfig = config;
        }

        public async Task SendAsync(object item, string queueName, TimeSpan? initialVisibilityDelay = null)
        {
            var queueClient = await CreateCloudQueueAsync(queueName);
            var queueMessage = await queueClient.SendMessageAsync(EncodeMessage(item), initialVisibilityDelay);
        }

        private static string EncodeMessage(object item)
        {
            var jsonMessage = JsonConvert.SerializeObject(item);

            var inputAsBytes = Encoding.UTF8.GetBytes(jsonMessage);
            return Convert.ToBase64String(inputAsBytes);
        }

        private async Task<QueueClient> CreateCloudQueueAsync(string queueName)
        {
            var cacheKey = $"CloudQueue|{queueName}";
            Exception cacheEx = null;
            var queue = await _cache.GetOrCreateAsync(cacheKey, e => Task.Run(async delegate
            {
                try
                {
                    e.SetSlidingExpiration(TimeSpan.FromMinutes(30));

                    var queueClient = new QueueClient(_infrastructureConfig.Storage.ConnectionString, queueName);
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

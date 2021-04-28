using System;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.CloudServices.CloudQueue
{
    public interface ICloudQueueService
    {
        Task SendAsync(object item, string queueName, TimeSpan? initialVisibilityDelay = null);
    }
}

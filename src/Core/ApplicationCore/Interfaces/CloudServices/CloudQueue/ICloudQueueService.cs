using System;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.CloudServices.CloudQueue
{
    public interface ICloudQueueService
    {
        Task SendAsync(object item, CloudQueueNames queueName, TimeSpan? initialVisibilityDelay = null);
    }
}

using System;
using System.Threading.Tasks;
using SubAndPub.Commons.Transport.Models;
using SubAndPub.Commons.Transport.Services.Params;

namespace SubAndPub.Commons.Transport.Services
{
    public interface ITransportService : IDisposable
    {
        Task PublishAsync<T>(IPublishMessageRequest<T> publishRequest);
        Task SubscribeAsync<T>(T subject, Func<Packet, Task> callback);
    }
}

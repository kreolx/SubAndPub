using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyNatsClient;
using MyNatsClient.Events;
using MyNatsClient.Rx;
using SubAndPub.Commons.Transport.Models;
using SubAndPub.Commons.Transport.Services.Params;

namespace SubAndPub.Commons.Transport.Services
{
    public class TransportService : ITransportService
    {
        private readonly NatsClient _client;
        private readonly ILogger<TransportService> _logger;
        public TransportService(ILogger<TransportService> logger, IOptions<NatsConnectionSettings> options)
        {
            _logger = logger;
            var natsConnectionSettings = options.Value;
            var connInfo = new ConnectionInfo(natsConnectionSettings.Url)
            {
                AutoReconnectOnFailure = natsConnectionSettings.AutoReconnectOnFailure,
            };
            _client = new NatsClient(connInfo);
            _client.Events.OfType<ClientConnected>()
                .Subscribe(ev => _logger.LogInformation("Client connected"));
            _client.ConnectAsync().Wait();
        }

        public async Task PublishAsync<T>(IPublishMessageRequest<T> request)
        {
            _logger.LogInformation("Publish message to broker");
            await _client.PubAsync(request.Subject.GetType().FullName, request.Message);
        }

        public async Task SubscribeAsync<T>(T subject, Func<Packet, Task> callback)
        {
            await _client.SubAsync(subject.GetType().FullName, stream => stream.SubscribeSafe(msg =>
            {
                callback(JsonSerializer.Deserialize<Packet>(msg.GetPayloadAsString()));
            }));
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}

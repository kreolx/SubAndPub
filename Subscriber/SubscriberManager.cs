using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SubAndPub.Commons.DAL.Services;
using SubAndPub.Commons.DAL.Services.Params;
using SubAndPub.Commons.Transport.Models;
using SubAndPub.Commons.Transport.Services;
using MT = SubAndPub.Commons.MessageTypes;

namespace Subscriber
{
    public class SubscriberManager : ISubscriberManager
    {
        private readonly IMessageService _messageService;
        private readonly ITransportService _transportService;
        private readonly ILogger<SubscriberManager> _logger;

        public SubscriberManager(IMessageService messageService, ITransportService transportService, ILogger<SubscriberManager> logger)
        {
            _messageService = messageService;
            _transportService = transportService;
            _logger = logger;
        }
        
        public async Task ExecuteAsync()
        {
            await _transportService.SubscribeAsync(new MT.Random(), SaveMessageAsync);
        }

        private async Task SaveMessageAsync(Packet message)
        {
            var saveMessageRequest = new MessageSaveRequest
            {
                Body = message.Body,
            };
            await _messageService.SaveMessageAsync(saveMessageRequest);
        }
    }
}

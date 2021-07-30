using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SubAndPub.Commons.DAL.Services;
using SubAndPub.Commons.DAL.Services.Params;
using SubAndPub.Commons.Transport.Models;
using SubAndPub.Commons.Transport.Services;
using SubAndPub.Commons.Transport.Services.Params;
using MT = SubAndPub.Commons.MessageTypes;

namespace Publisher
{
    public class PublisherManager : IPublisherManager
    {
        private readonly IMessageService _messageService;
        private readonly ITransportService _transportService;
        private readonly ILogger<PublisherManager> _logger;
        private readonly IMapper _mapper;

        public PublisherManager(IMessageService messageService, ITransportService transportService, ILogger<PublisherManager> logger, IMapper mapper)
        {
            _messageService = messageService;
            _transportService = transportService;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task ExecuteAsync()
        {
            while (true)
            {
                var messageRequest = new MessageSaveRequest
                {
                    Body = DateTime.Now.Ticks.ToString(),
                };
                var message = await _messageService.SaveMessageAsync(messageRequest);
                var publishRequest = new PublishMessageRequest<MT.Random>
                {
                    Subject = new MT.Random(),
                    Message = JsonSerializer.Serialize(_mapper.Map<Packet>(message)),
                };
                await _transportService.PublishAsync(publishRequest);
                await Task.Delay(1000);
            }
        }
    }
}

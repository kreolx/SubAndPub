using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SubAndPub.Commons.DAL.DbContexts;
using SubAndPub.Commons.DAL.Models;
using SubAndPub.Commons.DAL.Services.Params;

namespace SubAndPub.Commons.DAL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageDbContext _dbContext;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMessageDbContext dbContext, ILogger<MessageService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task<Message> SaveMessageAsync(IMessageSaveRequest saveRequest)
        {
            var hash = await GetHashCodeAsync();
            var message = new Message
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Body = saveRequest.Body,
                Hash = hash
            };
            _dbContext.Messages.Insert(message);
            _logger.LogInformation($"Message save: {message.Body} {message.CreatedAt}");
            return message;
        }

        public Task<List<Message>> GetMessagesAsync() =>
            Task.FromResult(_dbContext.Messages.Query()
                .OrderBy(d => d.CreatedAt)
                .ToList());
        
        private async Task<string> GetHashCodeAsync()
        {
            var messages = await GetMessagesAsync();
            
            var binaryFormatter = new BinaryFormatter();
            using(var stream = new MemoryStream())
            using (var md5 = MD5.Create())
            {
                binaryFormatter.Serialize(stream, messages);
                return BitConverter.ToString(md5.ComputeHash(stream.ToArray())).Replace("-", "");
            }
        }
    }
}

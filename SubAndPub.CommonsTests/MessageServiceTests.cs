using System;
using System.IO;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Logging;
using Moq;
using SubAndPub.Commons.DAL.DbContexts;
using SubAndPub.Commons.DAL.Models;
using SubAndPub.Commons.DAL.Services;
using SubAndPub.Commons.DAL.Services.Params;
using Xunit;

namespace SubAndPub.CommonsTests
{
    public class MessageServiceTests
    {
        [Fact]
        public async Task SaveMessageAsync_should_create_record()
        {
            var db = new LiteDatabase(new MemoryStream());
            var messagesCollection = db.GetCollection<Message>();
            var dbContext = new Mock<IMessageDbContext>();
            dbContext.Setup(d => d.Messages).Returns(() => messagesCollection);
            var messageService = new MessageService(dbContext.Object, new Mock<ILogger<MessageService>>().Object);
            var messageSaveRequest = new MessageSaveRequest
            {
                Body = "test message",
            };
            await messageService.SaveMessageAsync(messageSaveRequest);

            var message = messagesCollection.Query()
                .Where(d => d.Body.Equals(messageSaveRequest.Body))
                .FirstOrDefault();
            
            Assert.NotNull(message);
            Assert.Equal(message.CreatedAt.DateTime, DateTimeOffset.UtcNow.DateTime, TimeSpan.FromSeconds(1));
            Assert.NotEqual(default, message.Hash);
        }
        
        [Fact]
        public async Task GetMessages_should_return_list()
        {
            var db = new LiteDatabase(new MemoryStream());
            var messagesCollection = db.GetCollection<Message>();
            var dbContext = new Mock<IMessageDbContext>();
            dbContext.Setup(d => d.Messages).Returns(() => messagesCollection);
            messagesCollection.InsertBulk(new[]
            {
                new Message {Body = "1", CreatedAt = DateTimeOffset.UtcNow, Hash = "1"},
                new Message {Body = "2", CreatedAt = DateTimeOffset.UtcNow, Hash = "2"},
                new Message {Body = "3", CreatedAt = DateTimeOffset.UtcNow, Hash = "3"}
            });
            
            var messageService = new MessageService(dbContext.Object, new Mock<ILogger<MessageService>>().Object);
            var messages = await messageService.GetMessagesAsync();
            Assert.NotEmpty(messages);
            Assert.Collection(messages, message => Assert.Contains("1", message.Body),
                message => Assert.Contains("2", message.Body),
                message => Assert.Contains("3", message.Body));
        }
    }
}

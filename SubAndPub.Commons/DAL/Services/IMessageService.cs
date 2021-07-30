using System.Collections.Generic;
using System.Threading.Tasks;
using SubAndPub.Commons.DAL.Models;
using SubAndPub.Commons.DAL.Services.Params;

namespace SubAndPub.Commons.DAL.Services
{
    public interface IMessageService
    {
        Task<Message> SaveMessageAsync(IMessageSaveRequest saveRequest);
        Task<List<Message>> GetMessagesAsync();
    }
}

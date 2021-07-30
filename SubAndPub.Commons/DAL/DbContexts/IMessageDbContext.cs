using LiteDB;
using SubAndPub.Commons.DAL.Models;

namespace SubAndPub.Commons.DAL.DbContexts
{
    public interface IMessageDbContext
    {
        ILiteCollection<Message> Messages { get; }
    }
}

using System;
using LiteDB;
using Microsoft.Extensions.Options;
using SubAndPub.Commons.DAL.Models;

namespace SubAndPub.Commons.DAL.DbContexts
{
    public class MessageDbContext : IMessageDbContext, IDisposable
    {
        private readonly LiteDatabase _db;
        public ILiteCollection<Message> Messages => _db.GetCollection<Message>();
        
        public MessageDbContext(IOptions<DbConnectionSettings> _options)
        {
            var dbConnectionSettings = _options.Value;
            _db = new LiteDatabase(dbConnectionSettings.ConnectionString);
        }
        
        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}

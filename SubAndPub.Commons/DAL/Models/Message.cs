using System;

namespace SubAndPub.Commons.DAL.Models
{
    [Serializable]
    public class Message
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Body { get; set; }
        public string Hash { get; set; }
    }
}

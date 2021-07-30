using System;

namespace SubAndPub.Commons.Transport.Models
{
    public class Packet
    {
        public long Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Body { get; set; }
        public string Hash { get; set; }
    }
}

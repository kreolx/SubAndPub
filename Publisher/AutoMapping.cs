using SubAndPub.Commons.DAL.Models;
using SubAndPub.Commons.Transport.Models;

namespace Publisher
{
    public class AutoMapping : AutoMapper.Profile
    {
        public AutoMapping()
        {
            CreateMap<Message, Packet>();
        }
    }
}

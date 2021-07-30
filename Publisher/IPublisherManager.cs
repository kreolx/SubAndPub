using System.Threading.Tasks;

namespace Publisher
{
    public interface IPublisherManager
    {
        Task ExecuteAsync();
    }
}

namespace SubAndPub.Commons.Transport.Services.Params
{
    public interface IPublishMessageRequest<T>
    {
        T Subject { get; }
        string Message { get; }
    }
}

namespace SubAndPub.Commons.Transport.Services.Params
{
    public class PublishMessageRequest<T> : IPublishMessageRequest<T>
    {
        public T Subject { get; set; }
        public string Message { get; set; }
    }
}

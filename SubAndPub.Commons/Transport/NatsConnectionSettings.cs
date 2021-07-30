namespace SubAndPub.Commons.Transport
{
    public class NatsConnectionSettings
    {
        public string Url { get; set; }
        public bool AutoReconnectOnFailure  { get; set; }
    }
}

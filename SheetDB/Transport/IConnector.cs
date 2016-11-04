namespace SheetDB.Transport
{
    using System.Net;

    public interface IConnector
    {
        WebClient CreateRequest();
    }
}

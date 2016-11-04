namespace SheetDB.Transport
{
    using System.Net;

    public interface IRequestFactory
    {
        WebClient CreateRequest(string token);
    }
}

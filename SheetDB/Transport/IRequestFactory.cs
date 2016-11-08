namespace SheetDB.Transport
{
    using System.Net;

    public interface IRequestFactory
    {
        HttpWebRequest CreateRequest(string uri, string token);

        IResponse Send(HttpWebRequest request, HttpMethod method, string payload);
    }
}

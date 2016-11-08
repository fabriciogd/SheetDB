namespace SheetDB.Transport
{
    using System.Net;

    public interface IConnector
    {
        HttpWebRequest CreateRequest(string uri);

        IResponse Send(HttpWebRequest request, HttpMethod method, string payload = "");
    }
}

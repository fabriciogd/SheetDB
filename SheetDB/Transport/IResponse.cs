namespace SheetDB.Transport
{
    using System.Net;

    public interface IResponse
    {
        HttpStatusCode Status { get; }

        WebHeaderCollection Headers { get; }

        string Payload { get; }

        T Data<T>();
    }
}

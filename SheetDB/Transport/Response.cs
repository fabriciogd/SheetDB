namespace SheetDB.Transport
{
    using Newtonsoft.Json;
    using System.Net;

    public class Response : IResponse
    {
        public Response(HttpStatusCode status, WebHeaderCollection headers, string payload)
        {
            this.Status = status;
            this.Headers = headers;
            this.Payload = payload;
        }

        public WebHeaderCollection Headers { get; private set; }

        public string Payload { get; private set; }

        public HttpStatusCode Status { get; private set; }

        public T Data<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.Payload);
        }
    }
}

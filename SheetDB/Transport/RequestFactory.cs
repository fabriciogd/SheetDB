namespace SheetDB.Transport
{
    using System.IO;
    using System.Net;
    using System.Text;

    public class RequestFactory : IRequestFactory
    {
        public HttpWebRequest CreateRequest(string uri, string token)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

            request.ContentType = "application/json";

            request.Headers.Add("Authorization", "Bearer " + token);
            //request.Headers.Add("GData-Version", "3.0");

            return request;
        }

        public IResponse Send(HttpWebRequest request, HttpMethod method, string payload)
        {
            request.Method = method.ToString().ToUpper();

            if (!string.IsNullOrEmpty(payload))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(payload);
                request.ContentLength = bytes.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                return new Response(response.StatusCode, response.Headers, this.Convert(response));
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        private string Convert(HttpWebResponse response)
        {
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}

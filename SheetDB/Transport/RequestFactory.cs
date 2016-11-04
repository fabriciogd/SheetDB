namespace SheetDB.Transport
{
    using System.Net;
    using System.Text;

    /// <summary>
    /// The HTTP request factory class.
    /// </summary>
    public class RequestFactory : IRequestFactory
    {
        /// <summary>
        /// Creates a <see cref="WebClient"/> object with the current token
        /// </summary>
        /// <returns>Returns an instance of <see cref="WebClient"/></returns>
        public WebClient CreateRequest(string token)
        {
            var http = new WebClient();

            http.Encoding = Encoding.UTF8;
            http.Headers.Add("Authorization", "Bearer " + token);
            http.Headers.Add("Content-Type", "application/atom+xml; charset=UTF-8");
            http.Headers.Add("GData-Version", "3.0");

            return http;
        }
    }
}

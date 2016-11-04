namespace SheetDB.Transport
{
    /// <summary>
    /// Connector factory
    /// See more in <see href="https://developers.google.com/identity/protocols/OAuth2ServiceAccount">OAuth2ServiceAccount</see>
    /// </summary>
    public class ConnectorFactory
    {
        /// <summary>
        /// Creates a connector.
        /// </summary>
        /// <param name="clientEmail">Email address of the service account</param>
        /// <param name="privateKey">Private key for access google services</param>
        /// <returns>Connector created</returns>
        public static IConnector Create(string clientEmail, byte[] privateKey)
        {
            return Create(new RequestFactory(), clientEmail, privateKey);
        }

        internal static IConnector Create(IRequestFactory request, string clientEmail, byte[] privateKey)
        {
            return new Connector(request, clientEmail, privateKey);
        }
    }
}

namespace SheetDB.Transport
{
    public class ConnectorFactory
    {
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

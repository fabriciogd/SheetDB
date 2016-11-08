namespace SheetDB.Implementation
{
    using Helpers;
    using Newtonsoft.Json;
    using Transport;

    public class Managment : IManagment
    {
        private readonly IConnector _connector;

        public Managment(string clientEmail, byte[] privateKey)
        {
            this._connector = ConnectorFactory.Create(clientEmail, privateKey);
        }

        public IDatabase CreateDatabase(string name)
        {
            var uri = "https://www.googleapis.com/drive/v2/files?convert=true";

            var request = this._connector.CreateRequest(uri);

            request.ContentType = "application/json";

            var data = JsonConvert.SerializeObject(new
            {
                title = name,
                mimeType = "application/vnd.google-apps.spreadsheet"
            });

            IResponse response = this._connector.Send(request, HttpMethod.Post, data);

            return new Database();
        }

        public IDatabase GetDatabase(string name)
        {
            var uri = string.Format("https://www.googleapis.com/drive/v2/files?q={0}", Encode.UrlEncode(string.Format("title = \"{0}\"", name)));

            var request = this._connector.CreateRequest(uri);

            IResponse response = this._connector.Send(request, HttpMethod.Get, "");

            dynamic data = response.Data<dynamic>();

            var documents = data.items;

            if (documents.Count == 0)
                return null;

            var id = (string)documents.First.id;

            return new Database();
        }
    }
}

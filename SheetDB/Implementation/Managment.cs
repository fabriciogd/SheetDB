namespace SheetDB.Implementation
{
    using Helpers;
    using Newtonsoft.Json;
    using System.Collections.Generic;
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

            var payload = JsonConvert.SerializeObject(new
            {
                title = name,
                mimeType = "application/vnd.google-apps.spreadsheet"
            });

            IResponse response = this._connector.Send(request, HttpMethod.Post, payload);

            dynamic data = response.Data<dynamic>();

            var spreadsheetId = (string)data.id;

            return new Database(this._connector, spreadsheetId);
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

            var spreadsheetId = (string)documents.First.id;

            return new Database(this._connector, spreadsheetId);
        }

        public IEnumerable<IDatabase> GetAllDatabases()
        {
            var uri = string.Format("https://www.googleapis.com/drive/v2/files?q={0}", Encode.UrlEncode(string.Format("mimeType = \"{0}\"", "application/vnd.google-apps.spreadsheet")));

            var request = this._connector.CreateRequest(uri);

            IResponse response = this._connector.Send(request, HttpMethod.Get);

            dynamic data = response.Data<dynamic>();

            var documents = data.items;

            foreach (var document in documents)
                yield return new Database(this._connector, (string)document.id);
        }
    }
}

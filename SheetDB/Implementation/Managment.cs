namespace SheetDB.Implementation
{
    using Helpers;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net;
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
            var database = this.GetDatabase(name);

            if (database != null)
                throw new Exception("Exists a database with the same name");

            var uri = "https://sheets.googleapis.com/v4/spreadsheets";

            var request = this._connector.CreateRequest(uri);

            var payload = JsonConvert.SerializeObject(new
            {
                properties = new
                {
                    title = name
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            dynamic data = response
                .Status(HttpStatusCode.OK)
                .Response.Data<dynamic>();

            var spreadsheetId = (string)data.spreadsheetId;

            return new Database(this._connector, spreadsheetId);
        }

        public IDatabase GetDatabase(string name)
        {
            var uri = string.Format("https://www.googleapis.com/drive/v3/files?q={0}", Encode.UrlEncode(string.Format("name = \"{0}\"", name)));

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Get));

            dynamic data = response
                .Status(HttpStatusCode.OK)
                .Response.Data<dynamic>();

            var documents = data.files;

            if (documents.Count == 0)
                return null;

            var spreadsheetId = (string)documents.First.id;

            return new Database(this._connector, spreadsheetId);
        }

        public IEnumerable<IDatabase> GetAllDatabases()
        {
            var uri = string.Format("https://www.googleapis.com/drive/v3/files?q={0}", Encode.UrlEncode(string.Format("mimeType = \"{0}\"", "application/vnd.google-apps.spreadsheet")));

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Get));

            dynamic data = response
                .Status(HttpStatusCode.OK)
                .Response.Data<dynamic>();

            var documents = data.items;

            foreach (var document in documents)
                yield return new Database(this._connector, (string)document.id);
        }
    }
}

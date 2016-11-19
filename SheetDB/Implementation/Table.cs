namespace SheetDB.Implementation
{
    using Helpers;
    using Newtonsoft.Json;
    using SheetDB.Transport;
    using System.Linq;
    using System.Net;

    public class Table<T> : ITable<T>
        where T : new()
    {
        private readonly IConnector _connector;

        private readonly string _spreadsheetId;

        private readonly string _worksheetId;

        private readonly string _name;

        public Table(IConnector connector, string name, string spreadsheetId, string worksheetId)
        {
            this._connector = connector;
            this._name = name;
            this._spreadsheetId = spreadsheetId;
            this._worksheetId = worksheetId;
        }

        public void Delete()
        {
            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}:batchUpdate", this._spreadsheetId);

            var request = this._connector.CreateRequest(uri);

            var payload = JsonConvert.SerializeObject(new
            {
                requests = new
                {
                    deleteSheet = new
                    {
                        sheetId = this._worksheetId
                    }
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            response
                .Status(HttpStatusCode.OK);
        }

        public ITable<T> Rename(string newName)
        {
            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}:batchUpdate", this._spreadsheetId);

            var request = this._connector.CreateRequest(uri);

            var payload = JsonConvert.SerializeObject(new
            {
                requests = new
                {
                    updateSheetProperties = new
                    {
                        properties = new
                        {
                            sheetId = this._worksheetId,
                            title = newName
                        },
                        fields = "title"
                    }
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            response
                .Status(HttpStatusCode.OK);

            return this;
        }

        public IRow<T> Add(T record)
        {
            var queryParameters = "insertDataOption=INSERT_ROWS&valueInputOption=USER_ENTERED";

            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}:append?{2}", this._spreadsheetId, this._name, queryParameters);

            var request = this._connector.CreateRequest(uri);

            var fields = Utils.GetFields<T>();

            var payload = JsonConvert.SerializeObject(new
            {
                values = new[] {
                    fields.Select(a => a.GetValue(record, null))
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            dynamic data = response
                 .Status(HttpStatusCode.OK)
                 .Response.Data<dynamic>();

            var range = (string)data.updates.updatedRange;

            return new Row<T>(this._connector, record, this._spreadsheetId, this._worksheetId, range);
        }
    }
}

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

        public Table(IConnector connector, string spreadsheetId, string worksheetId)
        {
            this._connector = connector;
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
            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}:batchUpdate", this._spreadsheetId);

            var request = this._connector.CreateRequest(uri);

            var fields = Utils.GetFields<T>();

            var payload = JsonConvert.SerializeObject(new
            {
                requests = new
                {
                    appendCells = new
                    {
                        sheetId = this._worksheetId,
                        rows = new[]
                        {
                            new {
                                values = fields.Select(a => new { userEnteredValue = new { stringValue = a.GetValue(record, null).ToString()} })
                            }
                        },
                        fields = "*"
                    }
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            dynamic data = response
                 .Status(HttpStatusCode.OK)
                 .Response.Data<dynamic>();

            return new Row<T>(record);
        }
    }
}

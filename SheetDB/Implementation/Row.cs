using Newtonsoft.Json;
using SheetDB.Transport;
using System.Net;

namespace SheetDB.Implementation
{
    public class Row<T> : IRow<T> where T : new()
    {
        private readonly IConnector _connector;
        public T Element { get; set; }

        private readonly string _spreadsheetId;

        private readonly string _range;

        public Row(IConnector connector, T element, string spreadsheetId, string range)
        {
            this._connector = connector;
            this.Element = element;
            this._spreadsheetId = spreadsheetId;
            this._range = range;
        }

        public void Delete()
        {
            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}:batchUpdate", this._spreadsheetId);

            var request = this._connector.CreateRequest(uri);

            var payload = JsonConvert.SerializeObject(new
            {
                requests = new
                {
                    deleteNamedRange = new
                    {
                        namedRangeId = this._range
                    }
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            response
                .Status(HttpStatusCode.OK);
        }
    }
}

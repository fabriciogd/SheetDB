namespace SheetDB.Implementation
{
    using Newtonsoft.Json;
    using SheetDB.Helpers;
    using SheetDB.Transport;
    using System.Linq;
    using System.Net;

    public class Row<T> : IRow<T> where T : new()
    {
        private readonly IConnector _connector;
        public T Element { get; set; }

        private readonly string _spreadsheetId;

        private readonly string _worksheetId;

        private readonly string _range;

        public Row(IConnector connector, T element, string spreadsheetId, string worksheetId, string range)
        {
            this._connector = connector;
            this.Element = element;
            this._spreadsheetId = spreadsheetId;
            this._worksheetId = worksheetId;
            this._range = range;
        }

        public void Delete()
        {
            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}:batchUpdate", this._spreadsheetId);

            var request = this._connector.CreateRequest(uri);

            var index = Utils.A1ToRowIndex(this._range);

            var payload = JsonConvert.SerializeObject(new
            {
                requests = new
                {
                    deleteDimension = new
                    {
                        range = new
                        {
                            sheetId = this._worksheetId,
                            dimension = "ROWS",
                            startIndex = index - 1,
                            endIndex = index,
                        }
                    }
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            response
                .Status(HttpStatusCode.OK);
        }

        public IRow<T> Update(T record)
        {
            var queryParameters = "valueInputOption=RAW";

            var a1Notation = Utils.A1Notation(this._range);

            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}?{2}", this._spreadsheetId, a1Notation, queryParameters);

            var request = this._connector.CreateRequest(uri);

            var fields = Utils.GetFields<T>();

            var payload = JsonConvert.SerializeObject(new
            {
                values = new[] {
                    fields.Select(a => a.GetValue(record, null).ToString())
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Put, payload));

            response
                 .Status(HttpStatusCode.OK);

            this.Element = record;

            return this;
        }
    }
}

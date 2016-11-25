namespace SheetDB.Implementation
{
    using Helpers;
    using Linq;
    using Newtonsoft.Json;
    using SheetDB.Transport;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

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

        public IList<IRow<T>> Find(Query query)
        {
            var serializedQuery = this.SerializeQuery(query);

            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}?{2}", this._spreadsheetId, this._worksheetId, serializedQuery);

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Get));

            return null;
        }

        public IRow<T> GetByIndex(int rowNumber)
        {
            var q = new Query
            {
                Count = 1,
                Start = rowNumber,
            };

            var results = Find(q);

            if (results.Count == 0)
                return null;

            return results[0];
        }


        private string SerializeQuery(Query q)
        {
            var b = new StringBuilder();

            if (q.Where != null)
                b.Append("sq=" + Encode.UrlEncode(q.Where) + "&");

            if (q.Start > 0)
                b.Append("start-index=" + q.Start + "&");

            if (q.Count > 0)
                b.Append("max-results=" + q.Count + "&");

            if (q.Order != null)
            {
                if (q.Order.ColumnName != null)
                    b.Append("orderby=column:" + Encode.UrlEncode(q.Order.ColumnName) + "&");

                if (q.Order.Descending)
                    b.Append("reverse=true");
            }

            return b.ToString();
        }
    }
}

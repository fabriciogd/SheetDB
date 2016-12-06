namespace SheetDB.Implementation
{
    using Helpers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using SheetDB.Transport;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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

        public IRow<T> GetByIndex(int rowNumber)
        {
            rowNumber = rowNumber + 1;

            var countFields = Utils.GetFields<T>().Count();

            var fieldIndex = ((char)('A' + countFields)).ToString();

            var range = string.Format("{0}!A{1}:{2}{3}", this._name, rowNumber, fieldIndex, rowNumber);

            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}?fields=values&majorDimension=ROWS", this._spreadsheetId, range);

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Get));

            dynamic data = response
                 .Status(HttpStatusCode.OK)
                 .Response.Data<dynamic>();

            var registro = this.DeserializeElement(data.values.First);

            return new Row<T>(this._connector, registro, this._spreadsheetId, this._worksheetId, range);
        }

        public List<IRow<T>> GetAll()
        {
            var countFields = Utils.GetFields<T>().Count();

            var fieldIndex = ((char)('A' + countFields)).ToString();

            var range = string.Format("{0}!A2:{1}", this._name, fieldIndex);

            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}/values/{1}?fields=values&majorDimension=ROWS", this._spreadsheetId, range);

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Get));

            dynamic data = response
                 .Status(HttpStatusCode.OK)
                 .Response.Data<dynamic>();

            JArray registros = data.values;

            return registros.Select((a, i) => new Row<T>(
                this._connector, 
                this.DeserializeElement((JArray)a), 
                this._spreadsheetId, 
                this._worksheetId,
                string.Format("{0}!A{1}:{2}{3}", this._name, i + 1, fieldIndex, i + 1))).ToList() as List<IRow<T>>;
        }

        public T DeserializeElement(JArray registro)
        {
            var setters = registro.Select((e, i) => new
            {
                property = typeof(T).GetProperties()[i],
                rawValue = e,
            })
            .Select(e => new
            {
                e.property,
                value = ConvertFrom(e.rawValue, e.property.PropertyType),
            });

            var r = new T();

            foreach (var setter in setters)
                setter.property.SetValue(r, setter.value, null);

            return r;
        }

        public object ConvertFrom(object value, Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                var nc = new NullableConverter(t);
                return nc.ConvertFrom(value);
            }
            return Convert.ChangeType(value, t);
        }
    }
}

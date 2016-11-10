﻿namespace SheetDB.Implementation
{
    using Enum;
    using Helpers;
    using Model;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Net;
    using Transport;

    public class Database : IDatabase
    {
        private readonly IConnector _connector;

        private readonly string _spreadsheetId;

        public Database(IConnector connector, string spreadsheetId)
        {
            this._connector = connector;
            this._spreadsheetId = spreadsheetId;
        }

        public ITable<T> GetTable<T>(string name) where T : new()
        {
            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}?includeGridData=false", this._spreadsheetId);

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Get));

            dynamic data = response
                   .Status(HttpStatusCode.OK)
                   .Response.Data<dynamic>();

            var sheets = data.sheets;

            if (sheets.Count > 0)
                foreach (var sheet in sheets)
                    if (sheet.properties.title == name)
                        return new Table<T>(this._connector, this._spreadsheetId, (string)sheet.properties.sheetId);

            return null;
        }

        public ITable<T> CreateTable<T>(string name) where T : new()
        {
            var uri = string.Format("https://sheets.googleapis.com/v4/spreadsheets/{0}:batchUpdate", this._spreadsheetId);

            var fields = Utils.GetFields<T>();

            var request = this._connector.CreateRequest(uri);

            var payload = JsonConvert.SerializeObject(new
            {
                requests = new
                {
                    addSheet = new
                    {
                        properties = new
                        {
                            title = name,
                            sheetType = "GRID",
                            gridProperties = new
                            {
                                rowCount = 1,
                                columnCount = fields.Count()
                            }
                        }
                    }
                }
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            dynamic data = response
                   .Status(HttpStatusCode.OK)
                   .Response.Data<dynamic>();

            var sheetId = (string)data.replies.First.addSheet.properties.sheetId;

            return new Table<T>(this._connector, this._spreadsheetId, sheetId);
        }

        public void Delete()
        {
            var uri = string.Format("https://www.googleapis.com/drive/v3/files/{0}", this._spreadsheetId);

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Delete));

            response
               .Status(HttpStatusCode.NoContent);
        }

        public IDatabase AddPermission(string email, Role role, SheetDB.Enum.Type type)
        {
            var uri = string.Format("https://www.googleapis.com/drive/v3/files/{0}/permissions", this._spreadsheetId);

            var request = this._connector.CreateRequest(uri);

            var payload = JsonConvert.SerializeObject(new
            {
                role = System.Enum.GetName(typeof(SheetDB.Enum.Role), role),
                type = System.Enum.GetName(typeof(SheetDB.Enum.Type), type),
                emailAddress = email
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

            response
               .Status(HttpStatusCode.OK);

            return this;
        }

        public IDatabasePermission GetPermission(string email)
        {
            var fields = Encode.UrlEncode("emailAddress,id,role,type");

            var uri = string.Format("https://www.googleapis.com/drive/v3/files/{0}/permissions?fields=permissions({1})", this._spreadsheetId, fields);

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Get));

            dynamic data = response
                  .Status(HttpStatusCode.OK)
                  .Response.Data<dynamic>();

            var permissions = data.permissions;

            if (permissions.Count > 0)
                foreach (var permission in permissions)
                    if (permission.emailAddress == email)
                    {
                        var role = (Role)System.Enum.Parse(typeof(Role), (string)permission.role, true);
                        var type = (Type)System.Enum.Parse(typeof(Type), (string)permission.type, true);

                        return new DatabasePermission(this._connector, new Permission(permission.emailAddress, role, type), this._spreadsheetId, (string)permission.id);
                    }

            return null;
        }
    }
}

//var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));

//dynamic data = response
//   .Status(HttpStatusCode.OK)
//   .Response.Data<dynamic>();

//var sheetId = (string)data.replies.First.addSheet.properties.sheetId;

//request = this._connector.CreateRequest(uri);

//payload = JsonConvert.SerializeObject(new
//            {
//    requests = new
//    {
//        appendCells = new
//        {
//            sheetId = sheetId,
//            rows = new[]
//            {
//                            new {
//                                values = fields.Select(a => new { userEnteredValue = new { stringValue = a.Name.ToLowerInvariant() } })
//                            }
//                        },
//            fields = "*"
//        }
//    }
//            });

//response = new ResponseValidator(this._connector.Send(request, HttpMethod.Post, payload));
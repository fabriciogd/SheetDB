namespace SheetDB.Implementation
{
    using Enum;
    using Helpers;
    using Newtonsoft.Json;
    using SheetDB.Model;
    using System.Net;
    using Transport;

    public class DatabasePermission : IDatabasePermission
    {
        private readonly IConnector _connector;

        public Permission Permission { get; set; }

        private readonly string _spreadsheetId;

        private readonly string _permissionId;

        public DatabasePermission(IConnector connector, Permission permission, string spreadsheetId, string permissionId)
        {
            this._connector = connector;
            this.Permission = permission;
            this._spreadsheetId = spreadsheetId;
            this._permissionId = permissionId;
        }

        public void Delete()
        {
            var uri = string.Format("https://www.googleapis.com/drive/v3/files/{0}/permissions/{1}", this._spreadsheetId, this._permissionId);

            var request = this._connector.CreateRequest(uri);

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Delete));

            response
              .Status(HttpStatusCode.NoContent);
        }

        public IDatabasePermission Update(Role role)
        {
            var fields = Encode.UrlEncode("role");

            var uri = string.Format("https://www.googleapis.com/drive/v3/files/{0}/permissions/{1}?fields={2}", this._spreadsheetId, this._permissionId, fields);

            var request = this._connector.CreateRequest(uri);

            var payload = JsonConvert.SerializeObject(new
            {
                role = System.Enum.GetName(typeof(SheetDB.Enum.Role), role)
            });

            var response = new ResponseValidator(this._connector.Send(request, HttpMethod.Patch, payload));

            var data = response
               .Status(HttpStatusCode.OK)
               .Response.Data<dynamic>();

            this.Permission.Role = (Role)System.Enum.Parse(typeof(Role), (string)data.role, true);

            return this;
        }
    }
}

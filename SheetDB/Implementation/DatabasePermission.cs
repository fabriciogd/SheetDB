namespace SheetDB.Implementation
{
    using SheetDB.Model;
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
    }
}

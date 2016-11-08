namespace SheetDB.Implementation
{
    using SheetDB.Transport;

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
    }
}

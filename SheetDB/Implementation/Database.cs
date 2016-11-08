namespace SheetDB.Implementation
{
    using System;
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

        public ITable<T> CreateTable<T>(string name) where T : new()
        {
            throw new NotImplementedException();
        }
    }
}

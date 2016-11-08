namespace SheetDB.Implementation
{
    using System;

    public class Database : IDatabase
    {
        public ITable<T> CreateTable<T>(string name) where T : new()
        {
            throw new NotImplementedException();
        }
    }
}

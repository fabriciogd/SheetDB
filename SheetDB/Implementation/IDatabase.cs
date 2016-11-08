namespace SheetDB.Implementation
{
    public interface IDatabase
    {
        ITable<T> CreateTable<T>(string name) where T : new();
    }
}

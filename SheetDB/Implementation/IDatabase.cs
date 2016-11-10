namespace SheetDB.Implementation
{
    using SheetDB.Enum;

    public interface IDatabase
    {
        IDatabase AddPermission(string email, Role role, Type type);

        IDatabasePermission GetPermission(string email);

        ITable<T> CreateTable<T>(string name) where T : new();

        ITable<T> GetTable<T>(string name) where T : new();

        void Delete();
    }
}

using System.Collections.Generic;

namespace SheetDB.Implementation
{
    public interface IManagment
    {
        IDatabase CreateDatabase(string name);

        IDatabase GetDatabase(string name);

        IEnumerable<IDatabase> GetAllDatabases();
    }
}

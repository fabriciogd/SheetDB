using SheetDB.Linq;
using System.Collections.Generic;

namespace SheetDB.Implementation
{
    public interface ITable<T>
    {
        void Delete();

        ITable<T> Rename(string newName);

        IRow<T> Add(T record);

        IList<IRow<T>> Find(Query query);

        IRow<T> GetByIndex(int rowNumber);
    }
}

using System.Collections.Generic;

namespace SheetDB.Implementation
{
    public interface ITable<T>
    {
        void Delete();

        ITable<T> Rename(string newName);

        IRow<T> Add(T record);

        IRow<T> GetByIndex(int rowNumber);

        List<IRow<T>> GetAll();
    }
}

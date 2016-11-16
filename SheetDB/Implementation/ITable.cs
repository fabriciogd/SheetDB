namespace SheetDB.Implementation
{
    public interface ITable<T>
    {
        void Delete();

        ITable<T> Rename(string newName);

        IRow<T> Add(T record);
    }
}

namespace SheetDB.Implementation
{
    public interface IRow<T>
    {
        void Delete();

        IRow<T> Update(T record);
    }
}

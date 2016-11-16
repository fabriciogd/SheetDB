namespace SheetDB.Implementation
{
    public class Row<T> : IRow<T> where T : new()
    {
        public T Element { get; set; }

        public Row(T element)
        {
            this.Element = element;
        }
    }
}

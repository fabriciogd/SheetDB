namespace SheetDB.Linq
{
    public class Query
    {
        /// <summary>
        /// Start index, for paging
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Record count to fetch, for paging
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Structured query
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// Sort order
        /// </summary>
        public Order Order { get; set; }
    }
}

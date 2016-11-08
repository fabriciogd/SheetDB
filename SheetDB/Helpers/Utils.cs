namespace SheetDB.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Utils
    {
        public static IEnumerable<PropertyInfo> GetFields<T>()
        {
            return typeof(T).GetProperties().Where(p => p.CanRead);
        }
    }
}

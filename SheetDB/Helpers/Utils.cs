namespace SheetDB.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public static class Utils
    {
        public static IEnumerable<PropertyInfo> GetFields<T>()
        {
            return typeof(T).GetProperties().Where(p => p.CanRead);
        }

        public static int A1ToRowIndex(string range)
        {
            var match = new Regex(@"[a-z](\d+)", RegexOptions.IgnoreCase).Match(range);

            return Convert.ToInt32(match.Groups[1].Value);
        }

        public static String A1Notation(string range)
        {
            var match = new Regex(@"(^[a-z]+\d*?![a-z]+\d+)", RegexOptions.IgnoreCase).Match(range);

            return Convert.ToString(match.Groups[1].Value);
        }
    }
}

namespace SheetDB.Helpers
{
    using System;
    using System.Text;

    public static class Encode
    {
        public static string UrlBase64Encode(string s)
        {
            var bytesValue = Encoding.UTF8.GetBytes(s);
            return UrlBase64Encode(bytesValue);
        }

        public static string UrlBase64Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .Replace("=", String.Empty)
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public static string UrlEncode(string s)
        {
            return Uri.EscapeDataString(s).Replace("%20", "+");
        }
    }
}

namespace SheetDB.Helpers
{
    using System;
    using System.Text;

    /// <summary>
    /// Class that assists to encode a url
    /// See more in <see href="http://stackoverflow.com/questions/26353710/how-to-achieve-base64-url-safe-encoding-in-c">How to achieve Base64 URL safe encoding in C#?</see>
    /// </summary>
    public static class Encode
    {
        /// <summary>
        /// Encode a URL in base64
        /// </summary>
        /// <param name="s">String url</param>
        /// <returns>Returns a URL in base64</returns>
        public static string UrlBase64Encode(string s)
        {
            var bytesValue = Encoding.UTF8.GetBytes(s);
            return UrlBase64Encode(bytesValue);
        }

        /// <summary>
        /// Encode a URL in base64
        /// </summary>
        /// <param name="bytes">Byte[] url</param>
        /// <returns>Returns a URL in base64</returns>
        public static string UrlBase64Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .Replace("=", String.Empty)
                .Replace('+', '-')
                .Replace('/', '_');
        }

        /// <summary>
        /// Excape string
        /// </summary>
        /// <param name="s">String url</param>
        /// <returns>Returns a excaped string</returns>
        public static string UrlEncode(string s)
        {
            return Uri.EscapeDataString(s).Replace("%20", "+");
        }
    }
}

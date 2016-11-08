namespace SheetDB.Authentication
{
    using System;

    public class OAuth2
    {
        public readonly string Token;

        public readonly DateTime Expiration;

        public OAuth2(string token, DateTime expiration)
        {
            this.Token = token;
            this.Expiration = expiration;
        }
    }
}

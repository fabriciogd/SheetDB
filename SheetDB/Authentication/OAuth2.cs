namespace SheetDB.Authentication
{
    using System;

    /// <summary>
    /// Class that represents a OAuth2 model
    /// </summary>
    public class OAuth2
    {
        /// <summary>
        /// Authentication token 
        /// </summary>
        public readonly string Token;

        /// <summary>
        /// Limit time to expirate the token
        /// </summary>
        public readonly DateTime Expiration;

        /// <summary>
        /// Initialize a new instance of <see cref="OAuth2"/>
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="expiration">Limit time to expirate the token</param>
        public OAuth2(string token, DateTime expiration)
        {
            this.Token = token;
            this.Expiration = expiration;
        }
    }
}

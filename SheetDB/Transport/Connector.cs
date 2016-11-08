namespace SheetDB.Transport
{
    using Authentication;
    using Helpers;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Net;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public class Connector : IConnector
    {
        private readonly string _clientEmail;

        private readonly byte[] _privateKey;

        private readonly string _tokenAddress = "https://accounts.google.com/o/oauth2/token";

        private readonly string _scope = string.Join(" ", new[] {
            "https://www.googleapis.com/auth/drive",
            "https://www.googleapis.com/auth/drive.file"
        });

        private readonly DateTime _zeroDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private OAuth2 _oauthToken;

        private readonly IRequestFactory _request;

        public Connector(IRequestFactory request, string clientEmail, byte[] privateKey)
        {
            this._request = request;
            this._clientEmail = clientEmail;
            this._privateKey = privateKey;
        }

        private string GetHeader()
        {
            return JsonConvert.SerializeObject(new
            {
                typ = "JWT",
                alg = "RS256",
            });
        }

        private string GetClaimSet(DateTime now)
        {
            return JsonConvert.SerializeObject(new
            {
                scope = this._scope,
                iss = this._clientEmail,
                aud = this._tokenAddress,
                exp = (int)(now - this._zeroDate + TimeSpan.FromHours(1)).TotalSeconds,
                iat = (int)(now - this._zeroDate).TotalSeconds
            });
        }

        private string GetComputedSignature(DateTime now)
        {
            var header = this.GetHeader();
            var claimSet = this.GetClaimSet(now);
            var privateKey = this.GetPrivateKey();

            using (var hashAlg = new SHA256Managed())
            {
                hashAlg.Initialize();

                var headerAndPayload = Encode.UrlBase64Encode(header) + "." + Encode.UrlBase64Encode(claimSet);

                var headerPayloadBytes = Encoding.ASCII.GetBytes(headerAndPayload);

                var signature = Encode.UrlBase64Encode(privateKey.SignData(headerPayloadBytes, hashAlg));

                return headerAndPayload + "." + signature;
            }
        }

        private RSACryptoServiceProvider GetPrivateKey()
        {
            var certificate = new X509Certificate2(this._privateKey, "notasecret", X509KeyStorageFlags.Exportable);

            var rsa = (RSACryptoServiceProvider)certificate.PrivateKey;

            var privateKey = new RSACryptoServiceProvider();

            privateKey.ImportCspBlob(rsa.ExportCspBlob(true));

            return privateKey;
        }

        private string RequestToken()
        {
            var http = new WebClient();

            var response = http.UploadValues(this._tokenAddress, new NameValueCollection {
                {"grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer"},
                {"assertion", this.GetComputedSignature(DateTime.UtcNow)},
            });

            return Encoding.UTF8.GetString(response);
        }

        public string GetToken()
        {
            // If the token is null or expired, request a new token
            if (this._oauthToken == null || this._oauthToken.Expiration < DateTime.Now)
            {
                var tokenResponse = this.RequestToken();
                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(tokenResponse);
                var accessToken = (string)response["access_token"];
                var expiresSeconds = (long)response["expires_in"];
                var expiration = DateTime.Now.AddSeconds(expiresSeconds);
                this._oauthToken = new OAuth2(accessToken, expiration);
            }

            // return a current token
            return this._oauthToken.Token;
        }

        public HttpWebRequest CreateRequest(string url)
        {
            return this._request.CreateRequest(url, this.GetToken());
        }

        public IResponse Send(HttpWebRequest request, HttpMethod method, string payload = "")
        {
            return this._request.Send(request, method, payload);
        }
    }
}

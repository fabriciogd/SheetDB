using System;
using System.Net;

namespace SheetDB.Transport
{
    public class ResponseValidator
    {
        public IResponse Response { get; private set; }

        public ResponseValidator(IResponse response)
        {
            Response = response;
        }

        public ResponseValidator Status(HttpStatusCode status)
        {
            if (!this.Response.Status.Equals(status))
            {
                throw new Exception(string.Format("Response has wrong StatusCode. Should be {0} but is {1}", (int)status, (int)this.Response.Status));
            }

            return this;
        }
    }
}

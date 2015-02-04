using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace F23.AzureMobileServices.OpenIDConnect
{
    internal class UserInfoResponse
    {
        public UserInfoResponse(string raw)
        {
            Raw = raw;

            try
            {
                JsonObject = JObject.Parse(raw);
                var claims = new List<Tuple<string, string>>();

                foreach (var x in JsonObject)
                {
                    claims.Add(Tuple.Create(x.Key, x.Value.ToString()));
                }

                Claims = claims;
            }
            catch (Exception ex)
            {
                IsError = true;
                ErrorMessage = ex.Message;
            }
        }

        public UserInfoResponse(HttpStatusCode statusCode, string httpErrorReason)
        {
            IsHttpError = true;
            HttpErrorStatusCode = statusCode;
            HttpErrorReason = httpErrorReason;
        }

        public string Raw { get; private set; }

        public JObject JsonObject { get; private set; }
        
        public IEnumerable<Tuple<string, string>> Claims { get; set; }

        public bool IsHttpError { get; private set; }
        
        public HttpStatusCode HttpErrorStatusCode { get; private set; }
        
        public string HttpErrorReason { get; private set; }

        public bool IsError { get; private set; }
        
        public string ErrorMessage { get; set; }
    }
}

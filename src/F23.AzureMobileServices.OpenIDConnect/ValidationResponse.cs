using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F23.AzureMobileServices.OpenIDConnect
{
    internal class ValidationResponse
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("sub")]
        public string SubjectIdentifier { get; set; }

        [JsonProperty("amr")]
        public string Amr { get; set; }

        [JsonProperty("auth_time")]
        public int AuthTime { get; set; }

        [JsonProperty("idp")]
        public string Idp { get; set; }

        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("aud")]
        public string Audience { get; set; }

        [JsonProperty("exp")]
        public int Expiration { get; set; }

        [JsonProperty("nbf")]
        public int NotBefore { get; set; }

        [JsonProperty("scope")]
        public List<string> Scopes { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ClientId = " + ClientId);
            sb.AppendLine("SubjectIdentifier = " + SubjectIdentifier);
            sb.AppendLine("Amr = " + Amr);
            sb.AppendLine("AuthTime = " + AuthTime);
            sb.AppendLine("Idp = " + Idp);
            sb.AppendLine("Issuer = " + Issuer);
            sb.AppendLine("Audience = " + Audience);
            sb.AppendLine("Expiration = " + Expiration);
            sb.AppendLine("NotBefore = " + NotBefore);
            sb.AppendLine("Scopes = " + Scopes);
            return base.ToString();
        }
    }
}

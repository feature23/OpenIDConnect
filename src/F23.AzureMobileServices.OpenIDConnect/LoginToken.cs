using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F23.AzureMobileServices.OpenIDConnect
{
    /// <summary>
    /// A default WebAPI parameter model for authenticating a user with a JSON Web Token.
    /// </summary>
    public class LoginToken
    {
        /// <summary>
        /// The JSON Web Token access token provided to you by the OpenID Connect login process.
        /// </summary>
        public string JwtToken { get; set; }
    }
}

using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace F23.AzureMobileServices.OpenIDConnect
{
    /// <summary>
    /// A base class for a Web API controller to handle OpenID Connect logins.
    /// </summary>
    [AuthorizeLevel(AuthorizationLevel.Anonymous)]
    public abstract class OpenIDConnectLoginControllerBase : ApiController
    {
        private readonly string _azureMasterKey;
        private readonly string _tokenValidationEndpointFormat;
        private readonly string _userInfoEndpoint;

        private readonly IServiceTokenHandler _tokenHandler;

        /// <summary>
        /// Creates a new OpenIDConnectLoginControllerBase.
        /// </summary>
        /// <param name="tokenHandler">The Azure Mobile Services security token handler.</param>
        /// <param name="masterKey">The master key for your Azure Mobile Service.</param>
        /// <param name="openIdConnectBaseUrl">The base URL of your OpenIDConnect provider.</param>
        public OpenIDConnectLoginControllerBase(IServiceTokenHandler tokenHandler, string masterKey, string openIdConnectBaseUrl)
        {
            _tokenHandler = tokenHandler;

            _azureMasterKey = masterKey;

            _tokenValidationEndpointFormat = openIdConnectBaseUrl + "/connect/accesstokenvalidation?token={0}";
            _userInfoEndpoint = openIdConnectBaseUrl + "/connect/userinfo";
        }

        /// <summary>
        /// Creates a new login provider. Override this to provide a custom LoginProvider implementation.
        /// </summary>
        /// <param name="tokenHandler">The Azure Mobile Services security token handler.</param>
        /// <returns>Returns a new LoginProvider instance.</returns>
        protected virtual LoginProvider CreateLoginProvider(IServiceTokenHandler tokenHandler)
        {
            return new OpenIDConnectLoginProvider<OpenIDConnectProviderCredentials>(tokenHandler);
        }

        /// <summary>
        /// A method that is called when a user successfully authenticates with OpenID Connect. Use this to create a new
        /// local user in the database, update last login audit records, etc.
        /// </summary>
        /// <param name="userIdentifier">The username of the authenticated user from OpenID Connect.</param>
        /// <param name="claimsIdentity">The claims identity containing all of the user's claims from the OpenID Connect provider.</param>
        /// <returns>Returns an awaitable async Task.</returns>
        protected virtual async Task UserLoginSuccessfulAsync(string userIdentifier, ClaimsIdentity claimsIdentity)
        {
            await Task.Yield();
        }

        /// <summary>
        /// The HTTP POST method for this Web API controller. This is called to authenticate with Azure Mobile Services
        /// with your OpenID Connect token, and retrieve back a zumo auth token to use for subsequent Azure Mobile Services requests.
        /// </summary>
        /// <param name="token">The token from the OpenID Connect provider.</param>
        /// <returns>Returns an awaitable async task that has a result of the HTTP response.</returns>
        public async Task<HttpResponseMessage> Post(LoginToken token)
        {
            var client = new WebClient();
            var tokenValid = true;

            ValidationResponse validationResponse = null;
            //validation token with idp
            try
            {
                var validationResponseJson =
                    client.DownloadString(new Uri(String.Format(_tokenValidationEndpointFormat, token.JwtToken)));

                validationResponse = JsonConvert.DeserializeObject<ValidationResponse>(validationResponseJson);
                Debug.WriteLine(validationResponse.ToString());
            }
            catch (WebException exception)
            {
                tokenValid = false;
                Debug.WriteLine(exception.Message);
            }

            if (!tokenValid) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to validate token");

            //get user info from idp using token

            var userInfoClient = new UserInfoClient(new Uri(_userInfoEndpoint), token.JwtToken);
            var userInfoResponse = await userInfoClient.GetAsync();
            Debug.WriteLine(userInfoResponse.Raw);

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, validationResponse.SubjectIdentifier));
            claimsIdentity.AddClaims(userInfoResponse.Claims.Select(i => new Claim(i.Item1, i.Item2)));

            await UserLoginSuccessfulAsync(validationResponse.SubjectIdentifier, claimsIdentity);

            var loginProvider = CreateLoginProvider(_tokenHandler);

            var loginResult = loginProvider.CreateLoginResult(claimsIdentity, _azureMasterKey);

            return this.Request.CreateResponse(HttpStatusCode.OK, loginResult);
        }
    }
}

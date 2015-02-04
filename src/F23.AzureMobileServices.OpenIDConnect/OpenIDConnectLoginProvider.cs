using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace F23.AzureMobileServices.OpenIDConnect
{
    /// <summary>
    /// A default implementation of an Azure Mobile Services login provider for OpenID Connect.
    /// Feel free to inherit from this class to provide custom behavior.
    /// </summary>
    /// <typeparam name="TCredentials">The type of ProviderCredentials to serialize.</typeparam>
    public class OpenIDConnectLoginProvider<TCredentials> : LoginProvider
        where TCredentials : ProviderCredentials, new()
    {
        /// <summary>
        /// The default provider name to use in the default implementation. Value is OpenIDConnect
        /// </summary>
        public const string DefaultProviderName = "OpenIDConnect";

        /// <summary>
        /// Creates a new OpenIDConnectLoginProvider. Defaults token lifetime to 30 days.
        /// </summary>
        /// <param name="tokenHandler">The token handler to pass to the base class.</param>
        public OpenIDConnectLoginProvider(IServiceTokenHandler tokenHandler)
            : this(tokenHandler, new TimeSpan(30, 0, 0, 0))
        {
        }

        /// <summary>
        /// Creates a new OpenIDConnectLoginProvider.
        /// </summary>
        /// <param name="tokenHandler">The token handler to pass to the base class.</param>
        /// <param name="tokenLifetime">The lifetime the token is valid for.</param>
        public OpenIDConnectLoginProvider(IServiceTokenHandler tokenHandler, TimeSpan tokenLifetime)
            : base(tokenHandler)
        {
            this.TokenLifetime = tokenLifetime;
        }

        /// <summary>
        /// The name of the login provider. Default is OpenIDConnect. Override to customize.
        /// </summary>
        public override string Name
        {
            get { return DefaultProviderName; }
        }

        /// <summary>
        /// This method does nothing in the default implementation. Override to customize behavior.
        /// </summary>
        /// <param name="appBuilder">Not used.</param>
        /// <param name="settings">Not used.</param>
        public override void ConfigureMiddleware(IAppBuilder appBuilder, ServiceSettingsDictionary settings)
        {
        }

        /// <summary>
        /// Creates the ProviderCredentials object from the given ClaimsIdentity claim values.
        /// </summary>
        /// <param name="claimsIdentity">The claims identity to build credentials from.</param>
        /// <returns>Returns a new instance of type TCredentials.</returns>
        public sealed override ProviderCredentials CreateCredentials(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }

            string username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var credentials = new TCredentials
            {
                UserId = this.TokenHandler.CreateUserId(this.Name, username)
            };

            SetCredentialPropertiesFromClaimsIdentity(credentials, claimsIdentity);

            return credentials;
        }

        /// <summary>
        /// Override this method to set any custom properties on the credentials object from values from the given ClaimsIdentity.
        /// </summary>
        /// <param name="credentials">The credentials object to set properties on.</param>
        /// <param name="claimsIdentity">The claims identity to get claims from.</param>
        protected virtual void SetCredentialPropertiesFromClaimsIdentity(TCredentials credentials, ClaimsIdentity claimsIdentity)
        {
        }
        
        /// <summary>
        /// Parses the given JObject and returns a deserialized TCredentials object.
        /// </summary>
        /// <param name="serialized">The serialized JSON object containing credentials.</param>
        /// <returns>Returns a deserialized instance of TCredentials.</returns>
        public sealed override ProviderCredentials ParseCredentials(JObject serialized)
        {
            if (serialized == null)
            {
                throw new ArgumentNullException("serialized");
            }

            return serialized.ToObject<TCredentials>();
        }
    }
}

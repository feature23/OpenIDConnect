using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F23.AzureMobileServices.OpenIDConnect
{
    /// <summary>
    /// A default implementation of ProviderCredentials that has a provider name of OpenIDConnect.
    /// </summary>
    public class OpenIDConnectProviderCredentials : ProviderCredentials
    {
        /// <summary>
        /// Creates a new OpenIDConnectProviderCredentials with the default provider name of OpenIDConnect
        /// </summary>
        public OpenIDConnectProviderCredentials()
            : this(OpenIDConnectLoginProvider<OpenIDConnectProviderCredentials>.DefaultProviderName)
        {
        }

        /// <summary>
        /// Creates a new OpenIDConnectProviderCredentials with the given provider name.
        /// </summary>
        /// <param name="providerName">The provider name to use.</param>
        public OpenIDConnectProviderCredentials(string providerName)
            : base(providerName)
        {
        }
    }
}

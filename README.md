# OpenID Connect Utilities
A set of utility libraries to ease OpenID Connect integration with Azure Mobile Services and Xamarin.

## Azure Mobile Services .NET Backend

To use the F23.AzureMobileServices.OpenIDConnect library, add a reference to the DLL and create a new Web API controller in your project that inherits from OpenIDConnectLoginControllerBase:

```C#
[AuthorizeLevel(AuthorizationLevel.Anonymous)]
public class OpenIDLoginController : OpenIDConnectLoginControllerBase
{
  public OpenIDLoginController(IServiceTokenHandler tokenHandler)
    : base(tokenHandler, AppSettings.MsMasterKey, AppSettings.IdpSiteUrl)
  {
  }
}
```

Where AppSettings.MsMasterKey is the master key string to your Azure Mobile Service, and AppSettings.IdpSiteUrl is the base URL of your OpenID Connect provider (i.e. Thinktecture Identity Server v3+).

Then, from your mobile app, invoke a POST to /api/OpenIDLogin with your JWT token provided by the OpenID Connect provider (after authentication), in JSON format in the body of the HTTP request:
```JSON
{"jwtToken":"... your JWT token from OpenID Connect here ..."}
```

If successful, this will return a zumo authentication token to use with subsequent Azure Mobile Services requests to controllers that are marked with `[AuthorizeLevel(AuthorizationLevel.User)]`.

To provide custom behavior upon successful authentication, override the `UserLoginSuccessfulAsync` method. This will provide you with the user's username as well as their `ClaimsIdentity` for accessing the claims passed over from the OpenID Connect provider.

## License
This code is licensed under the MIT License. The full license information can be found in the LICENSE file. Portions derived from jmichas' unlicensed gist: https://gist.github.com/jmichas/46b37235ae2b6058a820

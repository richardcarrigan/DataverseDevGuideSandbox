using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace ExampleConsoleApp
{
  /// <summary>
  /// Custom HTTP message handler that uses OAuth authentication through
  /// Microsoft Authentication Library (MSAL).
  /// </summary>
  class OAuthMessageHandler : DelegatingHandler
  {
    private AuthenticationHeaderValue authHeader;

    public OAuthMessageHandler(string serviceUrl, string clientId, string redirectUrl, HttpMessageHandler innerHandler)
      : base(innerHandler)
    {
      string apiVersion = "9.2";
      string webApiUrl = $"{serviceUrl}/api/data/v{apiVersion}/";

      // Build Microsoft.Identity.Client (MSAL) OAuth Token Request
      var authBuilder = PublicClientApplicationBuilder
        .Create(clientId)
        .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
        .WithRedirectUri(redirectUrl)
        .Build();
      var scope = serviceUrl + "//.default";
      string[] scopes = { scope };

      AuthenticationResult authBuilderResult;

      // Popup authentication dialog box to get token
      authBuilderResult = authBuilder.AcquireTokenInteractive(scopes).ExecuteAsync().Result;

      // Note that an Azure AD access token has finite lifetime, default expiration is 60 minutes.
      authHeader = new AuthenticationHeaderValue("Bearer", authBuilderResult.AccessToken);
    }

    protected override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      System.Threading.CancellationToken cancellationToken
    )
    {
      request.Headers.Authorization = authHeader;
      return base.SendAsync(request, cancellationToken);
    }
  }
}

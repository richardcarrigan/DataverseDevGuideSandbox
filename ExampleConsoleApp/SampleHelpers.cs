namespace ExampleConsoleApp
{
  /// <summary>
  /// Shared code for common operations used by many Power Apps samples.
  /// </summary>
  class SampleHelpers
  {
    // These sample application registration values are available for all online instances.
    // You can use these while running sample code, but you should get your own for your own apps
    public static string clientId = "51f81489-12ee-4a9e-aaae-a2591f45987d";
    public static string redirectUrl = "http://localhost";

    /// <summary>
    /// Method used to get a value from the connection string
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="parameter"></param>
    /// <returns>The value from the connection string that matches the parameter key value</returns>
    public static string GetParameterValueFromConnectionString(string connectionString, string parameter)
    {
      try
      {
        return connectionString.Split(';').Where(s => s.Trim().StartsWith(parameter)).FirstOrDefault().Split('=')[1];
      }
      catch (Exception)
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// Returns an HttpClient configured with an OAuthMessageHandler
    /// </summary>
    /// <param name="connectionString">The connection string to use.</param>
    /// <param name="clientId">The client id to use when authenticating.</param>
    /// <param name="redirectUrl">The redirect Url to use when authenticating.</param>
    /// <param name="version">The version of Web API to use. Defaults to version 9.2.</param>
    /// <returns>An HttpClient you can use to perform authenticated operations with the Web API.</returns>
    public static HttpClient GetHttpClient(
      string connectionString,
      string clientId,
      string redirectUrl,
      string version = "v9.2"
    )
    {
      string url = GetParameterValueFromConnectionString(connectionString, "Url");
      try
      {
        HttpMessageHandler messageHandler = new OAuthMessageHandler(
          url,
          clientId,
          redirectUrl,
          new HttpClientHandler()
        );

        HttpClient httpClient = new HttpClient(messageHandler)
        {
          BaseAddress = new Uri(string.Format("{0}/api/data/{1}/", url, version)),
          Timeout = new TimeSpan(0, 2, 0) // 2 minutes
        };

        return httpClient;
      }
      catch (Exception)
      {
        throw;
      }
    }

    /// <summary>Displays exception information to the console.</summary>
    /// <param name="ex">The exception to output</param>
    public static void DisplayException(Exception ex)
    {
      Console.WriteLine("The application terminated with an error.");
      Console.WriteLine(ex.Message);
      while (ex.InnerException != null)
      {
        Console.WriteLine("\t* {0}", ex.InnerException.Message);
        ex = ex.InnerException;
      }
    }
  }
}

using System.Configuration;

namespace ExampleConsoleApp
{
  partial class Program
  {
    static void Main(string[] args)
    {
      try
      {
        // Get configuration data from App.config connectionStrings
        string connectionString = ConfigurationManager.ConnectionStrings["Connect"].ConnectionString;

        using (
          HttpClient client = SampleHelpers.GetHttpClient(
            connectionString,
            SampleHelpers.clientId,
            SampleHelpers.redirectUrl
          )
        )
        {
          // Use the WhoAmI function
          WhoAmIResponse response = WhoAmI(client);
          Console.WriteLine("Your system user ID is: {0}", response.UserId);
          Console.WriteLine("Press any key to exit.");
          Console.ReadLine();
        }
      }
      catch (Exception ex)
      {
        SampleHelpers.DisplayException(ex);
        Console.WriteLine("Press any key to exit.");
        Console.ReadLine();
      }
    }
  }
}

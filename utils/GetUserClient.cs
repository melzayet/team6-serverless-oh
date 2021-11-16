using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
static class GetUserClient
{
    // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
    static readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

    public static async Task<string> GetUser(string id){
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try	
        {
            System.Net.Http.HttpResponseMessage response = await client.GetAsync("https://serverlessohapi.azurewebsites.net/api/GetUser?userId=" + id);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            
            Trace.WriteLine(responseBody);
            return responseBody;
        }
        catch(HttpRequestException e)
        {
            Trace.WriteLine("\nException Caught!");	
            Trace.WriteLine("Message :{0} ",e.Message);
            return "failed";
        }
    }
}
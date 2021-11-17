using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Host;



namespace IceCream.Rating
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
                HttpRequest req,
            [CosmosDB(
                databaseName: "BFYOC_Datastore",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosDBSetting",
                SqlQuery = "SELECT * FROM c WHERE c.id = {Query.ratingId}")] dto.Rating rating,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            if(rating == null)
                return new NotFoundObjectResult("rating does not exist");
            else return new OkObjectResult(rating);


        }
    }
}

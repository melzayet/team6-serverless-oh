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
using System.Linq;
using System.Collections.Generic;



namespace IceCream.Rating
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetRating/ratingId/{ratingId}")]
                HttpRequest req,
            [CosmosDB(
                databaseName: "BFYOC_Datastore",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosDBSetting",
                SqlQuery = "SELECT * FROM c WHERE c.id = {ratingId}")] IEnumerable<dto.Rating> ratings,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

             if(!ratings.Any())
                {                    
                    return new NotFoundObjectResult("No rating found");
                }
            else 
                {                 
                    return new OkObjectResult(ratings);
                }


        }
    }
}

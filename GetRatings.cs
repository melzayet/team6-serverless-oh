using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Host;



namespace IceCream.Rating
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
                HttpRequest req,
            [CosmosDB(
                databaseName: "BFYOC_Datastore",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosDBSetting",
                userId = "{Query.id}",
                SqlQuery = "SELECT * FROM c order by c._ts desc WHERE c.userId = {userId}")]
                //SqlQuery = "SELECT * FROM c WHERE c.userId = 'cc20a6fb-a91f-4192-874d-132493685376'")]
                IEnumerable<dto.Rating> ratings,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            Trace.WriteLine("Checkpoint1");

            if(ratings == null)
            {
                Trace.WriteLine("Checkpoint2");
                return new NotFoundObjectResult("No ratings found for userId {Query.userId}");
            }
            else
            {
                Trace.WriteLine("Checkpoint3");
                foreach (dto.Rating rating in ratings)
                {
                    log.LogInformation(rating.userId);
                }
                return new OkObjectResult(ratings);
            }
        }
    }
}

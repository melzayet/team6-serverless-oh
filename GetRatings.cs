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
using System.Linq;

namespace IceCream.Rating{
    public static class GetRatings    {

        [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetRatings/userId/{userid}")]
                HttpRequest req,
            [CosmosDB(
                databaseName: "BFYOC_Datastore",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosDBSetting",
                SqlQuery = "SELECT * FROM c WHERE c.userId = {userid}")]
                IEnumerable<dto.Rating> ratings,
            ILogger log)
        {
            
            log.LogInformation("C# HTTP trigger function processed a request.");
            Trace.WriteLine("Checkpoint1");
            if(!ratings.Any())
                {
                    Trace.WriteLine("Checkpoint2");
                    return new NotFoundObjectResult("No ratings found");
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
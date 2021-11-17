using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace IceCream.Rating
{
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async  Task<IActionResult> Run(
        
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "BFYOC_Datastore",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosDBSetting")] IAsyncCollector<object> ratings,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string userId = data?.userId;
            string productId =  data?.productId;
            int rating = data?.rating;
            string responseString = "";

            //validate user
            string userCheckResult = await GetUserClient.GetUser(userId);
            if(userCheckResult == "failed") responseString = "invalid user Id";
            
            //validate product
            string productCheckResult = await GetProductClient.GetProduct(productId);
           if(productCheckResult == "failed") responseString = "invalid product Id";

             //validate raring    (need check if it's a number)
            if(rating < 0 || rating > 5)  responseString = "invalid rating";
            Trace.WriteLine(userCheckResult); 

            if (responseString != string.Empty) return new OkObjectResult(responseString);

            var ratingObject = new dto.Rating
                {
                    id = Guid.NewGuid().ToString(),
                    rating = rating,
                    userId = userId,
                    productId = productId,
                    locationName = data?.locationName,
                    userNotes = data?.userNotes,
                    timestamp = DateTime.UtcNow.ToString()
                };

            await ratings.AddAsync(ratingObject);

          
           return new OkObjectResult(JsonConvert.SerializeObject(ratingObject));
        }
    }
}

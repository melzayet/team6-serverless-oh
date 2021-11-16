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
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string userId = "cc20a6fb-a91f-4192-874d-132493685376" ?? data?.userId;
            string productId = "75542e38-563f-436f-adeb-f426f1dabb5c" ?? data?.productId;
            string rating = "75542e38-563f-436f-adeb-f426f1dabb5c" ?? data?.rating;
            string responseString = "success";

            //validate user
            string userCheckResult = await GetUserClient.GetUser(userId);
            dynamic userObject = JsonConvert.DeserializeObject(userCheckResult);
            if(userObject?.userId != userId)  responseString = "invalid user Id";
            Trace.WriteLine(userCheckResult);            

            //validate product
            string productCheckResult = await GetProductClient.GetProduct(productId);
            dynamic productObject = JsonConvert.DeserializeObject(productCheckResult);
            if(productObject?.productId != productId)  responseString = "invalid product Id";
            Trace.WriteLine(productCheckResult);

             //validate raring    (need check if it's a number)
            if(Int32.Parse(rating) < 0 || Int32.Parse(rating) > 5)  responseString = "invalid rating";
            Trace.WriteLine(userCheckResult); 

            return new OkObjectResult(responseString);
        }
    }
}

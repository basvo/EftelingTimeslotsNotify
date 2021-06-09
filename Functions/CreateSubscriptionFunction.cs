using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Functions
{
    public static class CreateSubscriptionFunction
    {
        [FunctionName("CreateSubscriptionFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
             [CosmosDB(
                databaseName: "Subscriptions",
                collectionName: "Subscriptions",
                CreateIfNotExists = true,
                ConnectionStringSetting = "CosmosDBConnection")]
                IAsyncCollector<Subscription> subscriptionsOut,
            ILogger log)
        {
            log.LogInformation("CreateSubscriptionFunction function processed a request.");

            string email = req.Query["email"];
            string date = req.Query["date"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            email = email ?? data?.email;
            date = date ?? data?.date;

            if (String.IsNullOrEmpty(email) || string.IsNullOrEmpty(date))
            {
                return new BadRequestResult();
            }
            var subscription = new Subscription()
            {
                Email = email,
                Date = Convert.ToDateTime(date, System.Globalization.CultureInfo.InvariantCulture),
                LastModified = DateTime.Now
            };

            await subscriptionsOut.AddAsync(subscription);

            log.LogInformation($"Email={subscription.Email}, Date={subscription.Date.ToShortDateString()}");

            string responseMessage = $"The subscription for {subscription.Date.ToShortDateString()} has been created. A notification will be sent to {subscription.Email} when a timeslot becomes available.";

            return new OkObjectResult(responseMessage);
        }
    }
}

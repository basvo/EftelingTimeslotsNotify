using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionsEfteling
{
    /// <summary>
    /// Function to retrieve the available timeslots from the Efteling website and store it in a CosmosDB collection
    /// </summary>
    public static class RetrieveTimeslotsFunction
    {
        [FunctionName("RetrieveTimeslotsFunction")]
        public static async Task Run([TimerTrigger("0 */5 * * * *"
            #if DEBUG
                ,RunOnStartup= true
            #endif
            )]TimerInfo myTimer,
             [CosmosDB(
                databaseName: "TimeSlots",
                collectionName: "Slots",
                CreateIfNotExists = true,
                ConnectionStringSetting = "CosmosDBConnection")]
                IAsyncCollector<Timeslot> timeslotsOut, ILogger log)
        {
            log.LogInformation($"RetrieveTimeslotsFunction executed at: {DateTime.Now}");

            var processor = new TimeslotsProcessor();
            var webscrapeURL = Environment.GetEnvironmentVariable("WebscrapeURL");
            var timeslots = await processor.RetrieveFromWeb(webscrapeURL, log);

            foreach (Timeslot timeslot in timeslots)
            {
                log.LogInformation($"Date={timeslot.Date.ToShortDateString()}, Window={timeslot.Window}, Available={timeslot.AvailableSpots}");
                await timeslotsOut.AddAsync(timeslot);
            }
        }
    }
}

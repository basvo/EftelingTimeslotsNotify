using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FunctionsEfteling
{
    public class TimeslotsProcessor : ITimeslotsProcessor
    {
        /// <summary>
        /// Scrapes the webscrapeURL for timeslots
        /// </summary>
        /// <param name="webscrapeURL">URL for the Efteling available timeslots web page</param>
        /// <param name="log">Performs logging</param>
        /// <returns></returns>
        public async Task<List<Timeslot>> RetrieveFromWeb(string webscrapeURL, ILogger log)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            client.DefaultRequestHeaders.Accept.Clear();

            // Get the response
            var response = await client.GetStringAsync(webscrapeURL);

            // Use the HTML Agility pack to parse the response into Timeslot objects
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var timeslotRows = htmlDoc.DocumentNode.Descendants("tr")
                .Where(node => node.GetAttributeValue("class", "").Contains("border-top--taupe")).ToList();

            var timeslots = new List<Timeslot>();

            foreach (var timeslotRow in timeslotRows)
            {
                try
                {
                    // Read the required data 
                    var timeslotRowDate = timeslotRow.Descendants("td").Where(node => node.GetAttributeValue("class", "").Contains("pl pv- palm-pv-- palm-full-width uc--first-letter")).FirstOrDefault().InnerText;
                    var timeslotRowWindow = timeslotRow.Descendants("td").Where(node => node.GetAttributeValue("data-label", "").Contains("Tijdslot")).FirstOrDefault().InnerText;
                    var timeslotAvailable = timeslotRow.Descendants("td").Where(node => node.GetAttributeValue("data-label", "").Contains("Beschikbare plaatsen")).FirstOrDefault().InnerText.Trim().Replace("Last minute", "").Replace("+", "").Replace("\n", "");

                    // Create a timeslot
                    Timeslot timeslot = new Timeslot()
                    {
                        Date = timeslotRowDate == "Morgen" ? DateTime.Now.AddDays(1) : Convert.ToDateTime(timeslotRowDate, new CultureInfo("nl-NL")),
                        Window = timeslotRowWindow,
                        AvailableSpots = Convert.ToInt16(timeslotAvailable),
                        lastModified = DateTime.Now
                    };

                    // Add the timeslot to the list
                    timeslots.Add(timeslot);
                }
                catch (FormatException exFormat)
                {
                    log.LogError($"FormatException occured while parsing timeslot: {exFormat}");
                }
            }

            return timeslots;
        }
    }
}

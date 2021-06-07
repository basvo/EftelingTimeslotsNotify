using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FunctionsEfteling
{
    public interface ITimeslotsProcessor
    {
        Task<List<Timeslot>> RetrieveFromWeb(string webscrapeURL, ILogger log);
    }
}
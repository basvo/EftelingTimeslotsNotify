using Newtonsoft.Json;
using System;

namespace FunctionsEfteling
{

    public class Timeslot
    {
        [JsonProperty("id")]
        public string Id
        {
            get
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Date.ToShortDateString() + Window);
                return System.Convert.ToBase64String(plainTextBytes);
            }
        }
        public DateTime Date { get; set; }
        public string Window { get; set; }
        public int AvailableSpots { get; set; }
        public DateTime lastModified { get; set; }
    }
}

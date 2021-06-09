using Newtonsoft.Json;
using System;

namespace Functions
{
    public class Subscription
    {
        [JsonProperty("id")]
        public string Id
        {
            get
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Email + Date.ToShortDateString());
                return System.Convert.ToBase64String(plainTextBytes);
            }
        }

        public string Email { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastModified { get; set; }
    }
}
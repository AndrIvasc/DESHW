using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class Body
    {
        [JsonProperty("data")]
        public Data? Data { get; set; }
    }
}

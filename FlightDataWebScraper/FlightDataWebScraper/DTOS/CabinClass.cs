using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class CabinClass
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}

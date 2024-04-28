using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class AirportArrival
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("resident")]
        public bool Resident { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("zone")]
        public string? Zone { get; set; }

        [JsonProperty("image")]
        public string? Image { get; set; }
    }
}

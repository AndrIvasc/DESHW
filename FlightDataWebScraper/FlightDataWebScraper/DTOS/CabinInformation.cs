using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class CabinInformation
    {
        [JsonProperty("number")]
        public int Number { get; set; }

        [JsonProperty("baggageWeight")]
        public BaggageWeight? BaggageWeight { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}

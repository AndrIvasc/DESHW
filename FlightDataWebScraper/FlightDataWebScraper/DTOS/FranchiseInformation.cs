using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class FranchiseInformation
    {
        [JsonProperty("franchise")]
        public int? Franchise { get; set; }

        [JsonProperty("baggageWeight")]
        public BaggageWeight? BaggageWeight { get; set; }

        [JsonProperty("hiringSupported")]
        public bool? HiringSupported { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}

using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class TotalAvailability
    {
        [JsonProperty("recommendationId")]
        public int RecommendationId { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }
}

using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class AvailabilityFactor
    {
        [JsonProperty("availabilityProviderType")]
        public string? AvailabilityProviderType { get; set; }

        [JsonProperty("availabilityProviderReasonType")]
        public string? AvailabilityProviderReasonType { get; set; }
    }
}

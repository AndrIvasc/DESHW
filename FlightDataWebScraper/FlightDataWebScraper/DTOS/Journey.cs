using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class Journey
    {
        [JsonProperty("recommendationId")]
        public int RecommendationId { get; set; }

        [JsonProperty("identity")]
        public int Identity { get; set; }

        [JsonProperty("direction")]
        public string? Direction { get; set; }

        [JsonProperty("cabinClass")]
        public string? CabinClass { get; set; }

        [JsonProperty("importChild")]
        public decimal ImportChild { get; set; }

        [JsonProperty("importInfant")]
        public decimal ImportInfant { get; set; }

        [JsonProperty("importAdultResident")]
        public decimal ImportAdultResident { get; set; }

        [JsonProperty("importChildResident")]
        public decimal ImportChildResident { get; set; }

        [JsonProperty("importInfantResident")]
        public decimal ImportInfantResident { get; set; }

        [JsonProperty("discountAdultResident")]
        public decimal DiscountAdultResident { get; set; }

        [JsonProperty("discountChildResident")]
        public decimal DiscountChildResident { get; set; }

        [JsonProperty("discountInfantResident")]
        public decimal DiscountInfantResident { get; set; }

        [JsonProperty("importTaxAdl")]
        public decimal ImportTaxAdl { get; set; }

        [JsonProperty("importTaxChd")]
        public decimal ImportTaxChd { get; set; }

        [JsonProperty("importTaxInf")]
        public decimal ImportTaxInf { get; set; }

        [JsonProperty("classCode")]
        public string? ClassCode { get; set; }

        [JsonProperty("farebasisCode")]
        public string? FarebasisCode { get; set; }

        [JsonProperty("promotionLabel")]
        public object? PromotionLabel { get; set; }

        [JsonProperty("flights")]
        public List<Flight>? Flights { get; set; }

        [JsonProperty("businessJourneys")]
        public List<object>? BusinessJourneys { get; set; }

        [JsonProperty("passengersAvailable")]
        public int PassengersAvailable { get; set; }

        [JsonProperty("fareFamily")]
        public FareFamily? FareFamily { get; set; }

        [JsonProperty("franchiseInformation")]
        public FranchiseInformation? FranchiseInformation { get; set; }

        [JsonProperty("cabinInformation")]
        public CabinInformation? CabinInformation { get; set; }
    }
}

using Newtonsoft.Json;

namespace FlightDataWebScraper.DTOS
{
    public class Data
    {
        [JsonProperty("sessionId")]
        public string? SessionId { get; set; }

        [JsonProperty("availabilityId")]
        public string? AvailabilityId { get; set; }

        [JsonProperty("locale")]
        public string? Locale { get; set; }

        [JsonProperty("marketCode")]
        public string? MarketCode { get; set; }

        [JsonProperty("swirt")]
        public bool Swirt { get; set; }

        [JsonProperty("switax")]
        public bool Switax { get; set; }

        [JsonProperty("swisdto")]
        public bool Swisdto { get; set; }

        [JsonProperty("adultPax")]
        public int AdultPax { get; set; }

        [JsonProperty("childPax")]
        public int ChildPax { get; set; }

        [JsonProperty("infantPax")]
        public int InfantPax { get; set; }

        [JsonProperty("adultPaxResident")]
        public int AdultPaxResident { get; set; }

        [JsonProperty("childPaxResident")]
        public int ChildPaxResident { get; set; }

        [JsonProperty("infantPaxResident")]
        public int InfantPaxResident { get; set; }

        [JsonProperty("messageItemization")]
        public List<object>? MessageItemization { get; set; }

        [JsonProperty("serviceFee")]
        public int ServiceFee { get; set; }

        [JsonProperty("serviceFeeDiscount")]
        public int ServiceFeeDiscount { get; set; }

        [JsonProperty("serviceFeeResidentDiscount")]
        public int ServiceFeeResidentDiscount { get; set; }

        [JsonProperty("totalAvailabilities")]
        public List<TotalAvailability>? TotalAvailabilities { get; set; }

        [JsonProperty("journeys")]
        public List<Journey>? Journeys { get; set; }

        [JsonProperty("calendarJourneys")]
        public List<object>? CalendarJourneys { get; set; }

        [JsonProperty("journeyConstraint")]
        public List<object>? JourneyConstraint { get; set; }

        [JsonProperty("blockType")]
        public string? BlockType { get; set; }

        [JsonProperty("availabilityFactor")]
        public AvailabilityFactor? AvailabilityFactor { get; set; }

        [JsonProperty("showDiscounts")]
        public bool ShowDiscounts { get; set; }

        [JsonProperty("discountLabel")]
        public string? DiscountLabel { get; set; }

        [JsonProperty("swiservicefee")]
        public bool Swiservicefee { get; set; }

        [JsonProperty("availabilityZoneType")]
        public string? AvailabilityZoneType { get; set; }
    }
}

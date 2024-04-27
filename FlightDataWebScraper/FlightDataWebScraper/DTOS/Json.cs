using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataWebScraper.DTOS
{
    public class Json
    {

        public class Airport
        {
            public string Code { get; set; }
            public string Description { get; set; }
            public bool Resident { get; set; }
            public string Type { get; set; }
            public string Zone { get; set; }
            public string Image { get; set; }
        }

        public class Flote
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }

        public class TechnicalStop
        {
            public int NumberStops { get; set; }
            public List<string> AirportStops { get; set; }
        }

        public class CabinClass
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }

        public class Flight
        {
            public string Number { get; set; }
            public Airport AirportDeparture { get; set; }
            public Airport AirportArrival { get; set; }
            public string DateDeparture { get; set; }
            public string DateArrival { get; set; }
            public string GmtDateDeparture { get; set; }
            public string GmtDateArrival { get; set; }
            public string CompanyCode { get; set; }
            public string Operator { get; set; }
            public Flote Flote { get; set; }
            public TechnicalStop TechnicalStop { get; set; }
            public string TerminalDeparture { get; set; }
            public string TerminalArrival { get; set; }
            public CabinClass CabinClass { get; set; }
        }

        public class Journey
        {
            public int RecommendationId { get; set; }
            public int Identity { get; set; }
            public string Direction { get; set; }
            public string CabinClass { get; set; }
            public decimal ImportChild { get; set; }
            public decimal ImportInfant { get; set; }
            public decimal ImportAdultResident { get; set; }
            public decimal ImportChildResident { get; set; }
            public decimal ImportInfantResident { get; set; }
            public decimal DiscountAdultResident { get; set; }
            public decimal DiscountChildResident { get; set; }
            public decimal DiscountInfantResident { get; set; }
            public decimal ImportTaxAdl { get; set; }
            public decimal ImportTaxChd { get; set; }
            public decimal ImportTaxInf { get; set; }
            public string ClassCode { get; set; }
            public string FarebasisCode { get; set; }
            public string PromotionLabel { get; set; }
            public List<Flight> Flights { get; set; }
        }

        public class Availability
        {
            public int RecommendationId { get; set; }
            public double Total { get; set; }
        }

        public class Data
        {
            public string SessionId { get; set; }
            public string AvailabilityId { get; set; }
            public string Locale { get; set; }
            public string MarketCode { get; set; }
            public bool Swirt { get; set; }
            public bool Switax { get; set; }
            public bool Swisdto { get; set; }
            public int AdultPax { get; set; }
            public int ChildPax { get; set; }
            public int InfantPax { get; set; }
            public int AdultPaxResident { get; set; }
            public int ChildPaxResident { get; set; }
            public int InfantPaxResident { get; set; }
            public List<object> MessageItemization { get; set; }  // List<object> if itemization structure is unknown
            public decimal ServiceFee { get; set; }
            public decimal ServiceFeeDiscount { get; set; }
            public decimal ServiceFeeResidentDiscount { get; set; }
            public List<Availability> TotalAvailabilities { get; set; }
            public List<Journey> Journeys { get; set; }
        }

        public class Body
        {
            public Data Data { get; set; }
        }

        public class RootObject
        {
            public Body Body { get; set; }
        }
    }
}

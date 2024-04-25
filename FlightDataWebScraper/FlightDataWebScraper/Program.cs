using System.Formats.Asn1;
using System.Globalization;
using System.Net.Http;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var url = "http://homeworktask.infare.lt/search.php?from=MAD&to=FUE&depart=2024-05-09&return=2024-05-16";
        var filePath = @"C:\Users\andre\Desktop\FlightData.csv";

        using HttpClient client = new HttpClient();

        var scraper = new WebScraperHttp();
        string jsonData = await scraper.FetchDataAsync(url);

        try
        {
            dynamic desirializedObject = JsonConvert.DeserializeObject(jsonData); //var arba dynamic

            var outboundFlights = FilterFlights(desirializedObject, "MAD", "AUH");
            var inboundFlights = FilterFlights(desirializedObject, "AUH", "MAD");

            // Group outbound and inbound flights by price category
            var groupedOutboundFlights = GroupFlightsByPriceCategory(outboundFlights);
            var groupedInboundFlights = GroupFlightsByPriceCategory(inboundFlights);

            var roundtripFlightCombinations = MakeRoundtripCombinations(groupedOutboundFlights, groupedInboundFlights);

            var cheapestOptions = FindCheapestOptions(roundtripFlightCombinations);

            SaveToCsv(cheapestOptions, filePath);

            Console.WriteLine("Data saved to flight_prices_with_taxes.csv successfully.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static List<dynamic> FilterFlights(dynamic jsonData, string departureAirport, string arrivalAirport)
    {
        var journeys = jsonData.body.data.journeys;
        var filteredFlights = new List<dynamic>();

        foreach (var journey in journeys)
        {
            foreach (var flight in journey.flights)
            {
                var departureCode = flight.airportDeparture.code;
                var arrivalCode = flight.airportArrival.code;

                if (departureCode == departureAirport && arrivalCode == arrivalAirport)
                {
                    filteredFlights.Add(flight);
                }
            }
        }

        return filteredFlights;
    }

    public static List<List<dynamic>> GroupFlightsByPriceCategory(List<dynamic> flights)
    {
        var groupedFlights = flights.GroupBy(flight => flight.total)
                                    .Select(group => group.ToList())
                                    .ToList();
        return groupedFlights;
    }

    public static List<List<dynamic>> MakeRoundtripCombinations(List<List<dynamic>> outboundFlights, List<List<dynamic>> inboundFlights)
    {
        var roundtripFlightCombinations = new List<List<dynamic>>();

        foreach (var outboundGroup in outboundFlights)
        {
            foreach (var inboundGroup in inboundFlights)
            {
                var roundtripCombination = new List<dynamic>();
                roundtripCombination.AddRange(outboundGroup);
                roundtripCombination.AddRange(inboundGroup);
                roundtripFlightCombinations.Add(roundtripCombination);
            }
        }

        return roundtripFlightCombinations;
    }

    public static List<dynamic> FindCheapestOptions(List<List<dynamic>> roundtripFlightCombinations)
    {
        var cheapestOptions = new List<dynamic>();

        foreach (var combination in roundtripFlightCombinations)
        {
            decimal minPrice = decimal.MaxValue;
            dynamic cheapestFlight = null;

            // Find the cheapest flight in the combination
            foreach (var flight in combination)
            {
                decimal totalPrice = flight.total;
                if (totalPrice < minPrice)
                {
                    minPrice = totalPrice;
                    cheapestFlight = flight;
                }
            }

            if (cheapestFlight != null)
            {
                // Calculate taxes
                decimal tax = CalculateTax(cheapestFlight);

                // Create a PriceWithTaxes object and add it to the list
                var priceWithTax = new PriceWithTaxes
                {
                    FlightNumber = cheapestFlight.number,
                    DepartureAirport = cheapestFlight.airportDeparture.code,
                    ArrivalAirport = cheapestFlight.airportArrival.code,
                    DepartureDate = cheapestFlight.dateDeparture,
                    ArrivalDate = cheapestFlight.dateArrival,
                    Price = minPrice,
                    Tax = tax
                };
                cheapestOptions.Add(priceWithTax);
            }
        }

        return cheapestOptions;
    }

    public static decimal CalculateTax(dynamic flight)
    {
        decimal totalTax = 0.0m;

        foreach (var journey in flight.journeys)
        {
            decimal journeyTax = journey.importTaxAdl;
            totalTax += journeyTax;
        }

        return totalTax;
    }

    public static void SaveToCsv(List<PriceWithTaxes> data, string filePath)
    {
        try
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                // Write the header row
                csv.WriteField("Flight Number");
                csv.WriteField("Departure Airport");
                csv.WriteField("Arrival Airport");
                csv.WriteField("Departure Date");
                csv.WriteField("Arrival Date");
                csv.WriteField("Price");
                csv.WriteField("Tax");
                csv.NextRecord();

                // Write flight data with prices and taxes
                foreach (var priceWithTax in data)
                {
                    csv.WriteField(priceWithTax.FlightNumber);
                    csv.WriteField(priceWithTax.DepartureAirport);
                    csv.WriteField(priceWithTax.ArrivalAirport);
                    csv.WriteField(priceWithTax.DepartureDate);
                    csv.WriteField(priceWithTax.ArrivalDate);
                    csv.WriteField(priceWithTax.Price);
                    csv.WriteField(priceWithTax.Tax);
                    csv.NextRecord();
                }
            }
            Console.WriteLine($"Data has been saved to {filePath} successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving data to {filePath}: {ex.Message}");
        }
    }

}
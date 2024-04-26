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

            // Make roundtrip flight combinations for each price category
            var roundtripFlightCombinations = new List<List<dynamic>>();
            foreach (var outboundGroup in groupedOutboundFlights)
            {
                foreach (var inboundGroup in groupedInboundFlights)
                {
                    var roundtripCombination = new List<dynamic>();
                    roundtripCombination.AddRange(outboundGroup);
                    roundtripCombination.AddRange(inboundGroup);
                    roundtripFlightCombinations.Add(roundtripCombination);
                }
            }

            // Extract prices and calculate taxes for each combination
            var pricesWithTaxes = new List<PriceWithTaxes>();
            foreach (var recommendation in desirializedObject.body.data.totalAvailabilities)
            {
                decimal totalPrice = recommendation.total;
                // Assuming tax is 20% of the total price
                var tax = totalPrice * 0.2m;
                pricesWithTaxes.Add(new PriceWithTaxes { Price = totalPrice, Tax = tax });
            }

            // Save data to CSV file
            SaveToCsv(pricesWithTaxes, @"C:\Users\andre\Desktop\FlightData.csv");
            Console.WriteLine("Data saved to flight_prices_with_taxes.csv successfully.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static List<dynamic> FilterFlights(dynamic jsonData, string departureAirport, string arrivalAirport)
    {
        var journeys = jsonData.body.data.journeys;
        var filteredFlights = new List<dynamic>();

        foreach (var journey in journeys)
        {
            foreach (var flight in journey.flights)
            {
                var departureCode = flight.airportDeparture.code;
                var arrivalCode = flight.airportArrival.code;

                // Check if the flight matches the specified departure and arrival airports
                if (departureCode == departureAirport || arrivalCode == arrivalAirport)
                {
                    filteredFlights.Add(flight);
                }
            }
        }

        return filteredFlights;
    }

    static List<List<dynamic>> GroupFlightsByPriceCategory(List<dynamic> flights)
    {
        var groupedFlights = flights.GroupBy(flight => flight.total)
                                    .Select(group => group.ToList())
                                    .ToList();

        return groupedFlights;
    }

    static void SaveToCsv(List<PriceWithTaxes> data, string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(data);
        }
    }

}
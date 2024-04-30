using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDataWebScraper.Services
{
    public class CSVService
    {

        public void SaveToCsv(List<PriceWithTaxes> data, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteField("Flight Number");
                csv.WriteField("Departure Airport");
                csv.WriteField("Arrival Airport");
                csv.WriteField("Departure Date");
                csv.WriteField("Arrival Date");
                csv.WriteField("Price");
                csv.WriteField("Tax");
                csv.NextRecord();

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

    }
}

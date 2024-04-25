public class PriceWithTaxes
{
    public string? FlightNumber { get; set; }
    public string? DepartureAirport { get; set; }
    public string? ArrivalAirport { get; set; }
    public string DepartureDate { get; set; }
    public string? ArrivalDate { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
}
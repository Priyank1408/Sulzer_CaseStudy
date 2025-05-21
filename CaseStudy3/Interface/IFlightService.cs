using FlightRoutePlanner;

namespace CaseStudy3.Interface
{
    public interface IFlightService
    {
        List<Flight> GetFlights(string departureCity, string arrivalCity);
        double CalculateFlightPrice(Flight flight);
    }
}
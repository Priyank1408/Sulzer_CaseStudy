using CaseStudy3.Interface;
using FlightRoutePlanner;

namespace CaseStudy3.Services
{
    public class FlightService : IFlightService
    {
        private readonly List<Flight> _flights;
        private readonly Random _random = new Random();

        public FlightService(List<Flight> flights)
        {
            _flights = flights;
        }

        public List<Flight> GetFlights(string departureCity, string arrivalCity)
        {
            return _flights.Where(f => f.DepartureCity == departureCity && f.ArrivalCity == arrivalCity).ToList();
        }

        public double CalculateFlightPrice(Flight flight)
        {
            // Dynamic pricing logic based on time, availability, and promotions
            double basePrice = flight.Distance * 0.1;
            double dynamicFactor = _random.NextDouble() * 20; // Example dynamic factor
            return basePrice + dynamicFactor;
        }
    }
}
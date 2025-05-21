using FlightRoutePlanner;
namespace CaseStudy3.Interface
{
    public interface IRoutePlannerService
    {
        List<List<Flight>> FindCheapestRoute(string departureCity, string arrivalCity, int maxConnections = 2, TimeSpan? maxDuration = null);
    }
}
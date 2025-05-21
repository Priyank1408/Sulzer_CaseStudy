using CaseStudy3.Interface;
using FlightRoutePlanner;

namespace CaseStudy3.Services
{
    public class RoutePlannerService : IRoutePlannerService
    {
        private readonly IFlightService _flightService;

        public RoutePlannerService(IFlightService flightService)
        {
            _flightService = flightService;
        }

        public List<List<Flight>> FindCheapestRoute(string departureCity, string arrivalCity, int maxConnections = 2, TimeSpan? maxDuration = null)
        {
            List<List<Flight>> routes = new List<List<Flight>>();
            FindRoutesRecursive(departureCity, arrivalCity, new List<Flight>(), routes, maxConnections + 1, maxDuration);

            return routes.OrderBy(r => r.Sum(f => _flightService.CalculateFlightPrice(f))).ToList();
        }

        private void FindRoutesRecursive(string currentCity, string arrivalCity, List<Flight> currentRoute, List<List<Flight>> routes, int maxConnections, TimeSpan? maxDuration)
        {
            if (maxConnections <= 0) return;

            if (currentCity == arrivalCity)
            {
                if (currentRoute.Count > 0)
                {
                    if (maxDuration == null || currentRoute.Sum(f => f.Duration.TotalHours) <= maxDuration?.TotalHours)
                    {
                        routes.Add(new List<Flight>(currentRoute));
                    }
                }
                return;
            }

            var possibleFlights = _flightService.GetFlights(currentCity, null);

            foreach (var flight in possibleFlights)
            {
                List<Flight> newRoute = new List<Flight>(currentRoute);
                newRoute.Add(flight);
                FindRoutesRecursive(flight.ArrivalCity, arrivalCity, newRoute, routes, maxConnections - 1, maxDuration);
            }
        }
    }
}
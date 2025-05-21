using CaseStudy3.Interface;
using CaseStudy3.Services;
namespace FlightRoutePlanner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Sample Data
            List<Flight> flights = new List<Flight>
            {
                new Flight { DepartureCity = "City_A", ArrivalCity = "City_B", Distance = 500, PriceCalculator = () => 500 * 0.1 + new Random().NextDouble() * 20 },
                new Flight { DepartureCity = "City_B", ArrivalCity = "City_C", Distance = 300, PriceCalculator = () => 300 * 0.1 + new Random().NextDouble() * 20 },
                new Flight { DepartureCity = "City_A", ArrivalCity = "City_C", Distance = 800, PriceCalculator = () => 800 * 0.1 + new Random().NextDouble() * 20 },
                new Flight { DepartureCity = "City_A", ArrivalCity = "City_D", Distance = 600, PriceCalculator = () => 600 * 0.1 + new Random().NextDouble() * 20 },
                new Flight { DepartureCity = "City_D", ArrivalCity = "City_C", Distance = 200, PriceCalculator = () => 200 * 0.1 + new Random().NextDouble() * 20 }
            };

            // Dependency Injection
            IFlightService flightService = new FlightService(flights);
            IRoutePlannerService routePlannerService = new RoutePlannerService(flightService);

            // Find Cheapest Route
            var cheapestRoutes = routePlannerService.FindCheapestRoute("City_A", "City_C");

            if (cheapestRoutes.Any())
            {
                Console.WriteLine("Cheapest Routes:");
                foreach (var route in cheapestRoutes)
                {
                    double totalPrice = route.Sum(f => flightService.CalculateFlightPrice(f));
                    Console.WriteLine($"Route: {string.Join(" -> ", route.Select(f => f.DepartureCity))} -> {route.Last().ArrivalCity}, Total Price: ${totalPrice:F2}");
                }
            }
            else
            {
                Console.WriteLine("No routes found.");
            }

            Console.ReadKey();
        }
    }
}
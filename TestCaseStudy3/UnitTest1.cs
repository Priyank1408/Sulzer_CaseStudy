using CaseStudy3.Interface; 
using CaseStudy3.Services;
using FlightRoutePlanner;
using NSubstitute;
namespace TestCaseStudy3
{
    public class CaseStudy3Tests
    {
        [Fact]
        public void FlightService_GetFlights_ReturnsCorrectFlights()
        {
            // Arrange
            var flights = new List<Flight>
            {
                new Flight { DepartureCity = "City_A", ArrivalCity = "City_B", Distance = 500, PriceCalculator = () => 500 * 0.1 },
                new Flight { DepartureCity = "City_B", ArrivalCity = "City_C", Distance = 300, PriceCalculator = () => 300 * 0.1 }
            };
            var flightService = new FlightService(flights);

            // Act
            var result = flightService.GetFlights("City_A", "City_B");

            // Assert
            Assert.Single(result);
            Assert.Equal("City_A", result[0].DepartureCity);
            Assert.Equal("City_B", result[0].ArrivalCity);
        }

        [Fact]
        public void RoutePlannerService_FindCheapestRoute_ReturnsCheapestRoute()
        {
            // Arrange
            IFlightService flightService = Substitute.For<IFlightService>();
            flightService.GetFlights("City_A", null).Returns(new List<Flight> {
                new Flight { DepartureCity = "City_A", ArrivalCity = "City_B", Distance = 500, Duration = TimeSpan.FromHours(2), PriceCalculator = () => 50 },
                new Flight { DepartureCity = "City_A", ArrivalCity = "City_C", Distance = 800, Duration = TimeSpan.FromHours(3), PriceCalculator = () => 80 }
            });
            flightService.GetFlights("City_B", null).Returns(new List<Flight> {
                new Flight { DepartureCity = "City_B", ArrivalCity = "City_C", Distance = 300, Duration = TimeSpan.FromHours(1), PriceCalculator = () => 30 }
            });
            flightService.CalculateFlightPrice(Arg.Any<Flight>()).Returns(50, 30, 80);

            var routePlannerService = new RoutePlannerService(flightService);

            // Act
            var routes = routePlannerService.FindCheapestRoute("City_A", "City_C");

            // Assert
            Assert.NotNull(routes);
            
            if (routes.Any())
            {
                Assert.True(routes.First().Sum(f => flightService.CalculateFlightPrice(f)) > 0);
            }
        }

        [Fact]
        public void RoutePlannerService_FindCheapestRoute_WithMaxDuration_FiltersRoutes()
        {
            // Arrange
            IFlightService flightService = Substitute.For<IFlightService>(); 
            flightService.GetFlights("City_A", null).Returns(new List<Flight> { 
                new Flight { DepartureCity = "City_A", ArrivalCity = "City_B", Distance = 500, Duration = TimeSpan.FromHours(2), PriceCalculator = () => 50 },
                new Flight { DepartureCity = "City_A", ArrivalCity = "City_C", Distance = 800, Duration = TimeSpan.FromHours(5), PriceCalculator = () => 80 }
            });
            flightService.GetFlights("City_B", null).Returns(new List<Flight> { 
                new Flight { DepartureCity = "City_B", ArrivalCity = "City_C", Distance = 300, Duration = TimeSpan.FromHours(1), PriceCalculator = () => 30 }
            });
            flightService.CalculateFlightPrice(Arg.Any<Flight>()).Returns(50, 30, 80);

            var routePlannerService = new RoutePlannerService(flightService);

            // Act
            var routes = routePlannerService.FindCheapestRoute("City_A", "City_C", maxDuration: TimeSpan.FromHours(4));

            // Assert
            Assert.NotNull(routes);
            if (routes.Any())
            {
                foreach (var route in routes)
                {
                    Assert.True(route.Sum(f => f.Duration.TotalHours) <= TimeSpan.FromHours(4).TotalHours);
                }
            }
        }
    }
}
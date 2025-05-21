namespace FlightRoutePlanner
{
    public class Flight
    {
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public double Distance { get; set; }
        public TimeSpan Duration { get; set; }
        public Func<double> PriceCalculator { get; set; }

        public double GetPrice()
        {
            return PriceCalculator();
        }
    }
}
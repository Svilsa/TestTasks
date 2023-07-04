using System;
using Gridnine.FlightCodingTest.Filters;

namespace Gridnine.FlightCodingTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var filterCase1 = new IFlightFilter[] {new BeforeNowDepartureFlightFilter()};
            var filterCase2 = new IFlightFilter[] {new ArrivalBeforeDepartureFilter()};
            var filterCase3 = new IFlightFilter[] {new TwoHoursDelayFilter()};

            var filterExtraCase = new IFlightFilter[]
                {new BeforeNowDepartureFlightFilter(), new ArrivalBeforeDepartureFilter(), new TwoHoursDelayFilter()};

            var flightBuilder = new FlightBuilder();
            var flights = flightBuilder.GetFlights();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nBeforeNowDepartureFlightFilter Results:");
            Console.ResetColor();
            foreach (var flight in flights.Filter(filterCase1)) flight.DisplayInfo();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nArrivalBeforeDepartureFilter Results:");
            Console.ResetColor();
            foreach (var flight in flights.Filter(filterCase2)) flight.DisplayInfo();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nTwoHoursDelayFilter Results:");
            Console.ResetColor();
            foreach (var flight in flights.Filter(filterCase3)) flight.DisplayInfo();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nAll Filters Results:");
            Console.ResetColor();
            foreach (var flight in flights.Filter(filterExtraCase)) flight.DisplayInfo();
        }
    }
}
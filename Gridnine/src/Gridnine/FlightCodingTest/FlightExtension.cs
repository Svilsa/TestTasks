using System;

namespace Gridnine.FlightCodingTest
{
    public static class FlightExtension
    {
        public static void DisplayInfo(this Flight flight)
        {
            Console.WriteLine("Flight:");
            foreach (var segment in flight.Segments)
            {
                Console.WriteLine($"\tDeparture at {segment.DepartureDate}");
                Console.WriteLine($"\tArrival at {segment.ArrivalDate}");
                Console.WriteLine("\t" + new string('-', 30));
            }

            Console.WriteLine(new string('#', 40));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gridnine.FlightCodingTest.Filters
{
    public class TwoHoursDelayFilter : IFlightFilter
    {
        public IEnumerable<Flight> Filter(IEnumerable<Flight> flights)
        {
            return flights
                .Where(flight => !IsTwoHourDelay(flight));
        }

        private static bool IsTwoHourDelay(Flight flight)
        {
            var counter = new TimeSpan();

            for (var i = 0; i < flight.Segments.Count - 1; i++)
            {
                var currentSegment = flight.Segments[i];
                var nextSegment = flight.Segments[i + 1];

                counter = counter.Add(nextSegment.DepartureDate - currentSegment.ArrivalDate);

                if (counter.Hours > 2) return true;
            }

            return false;
        }
    }
}
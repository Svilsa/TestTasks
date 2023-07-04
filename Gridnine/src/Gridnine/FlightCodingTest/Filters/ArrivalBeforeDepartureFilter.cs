using System.Collections.Generic;
using System.Linq;

namespace Gridnine.FlightCodingTest.Filters
{
    public class ArrivalBeforeDepartureFilter : IFlightFilter
    {
        public IEnumerable<Flight> Filter(IEnumerable<Flight> flights)
        {
            return flights
                .Where(flight => flight.Segments
                    .All(s => s.ArrivalDate > s.DepartureDate)
                );
        }
    }
}
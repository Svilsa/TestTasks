using System;
using System.Collections.Generic;
using System.Linq;

namespace Gridnine.FlightCodingTest.Filters
{
    public class BeforeNowDepartureFlightFilter : IFlightFilter
    {
        public IEnumerable<Flight> Filter(IEnumerable<Flight> flights)
        {
            return flights
                .Where(flight => flight.Segments
                        .OrderBy(s => s.DepartureDate)
                        .First().DepartureDate > DateTime.Now
                );
        }
    }
}
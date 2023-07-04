using System.Collections.Generic;
using System.Linq;
using Gridnine.FlightCodingTest.Filters;

namespace Gridnine.FlightCodingTest
{
    public static class FlightFilterExtension
    {
        public static IEnumerable<Flight> Filter(this IEnumerable<Flight> flights, IEnumerable<IFlightFilter> filters)
        {
            return filters.Aggregate(flights, (current, filter) => filter.Filter(current));
        }
    }
}
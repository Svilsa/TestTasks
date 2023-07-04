using System.Collections.Generic;

namespace Gridnine.FlightCodingTest.Filters
{
    public interface IFlightFilter
    {
        public IEnumerable<Flight> Filter(IEnumerable<Flight> flights);
    }
}
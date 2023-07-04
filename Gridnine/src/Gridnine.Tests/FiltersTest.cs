using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gridnine.FlightCodingTest;
using Gridnine.FlightCodingTest.Filters;
using Xunit;

namespace Gridnine.Tests
{
    public class FiltersTest
    {
        [Fact]
        public void BeforeNowDepartureFlightFilterTest()
        {
            var filter = new IFlightFilter[] {new BeforeNowDepartureFlightFilter()};

            #region TestVariables

            var flights = new List<Flight>
            {
                new()
                {
                    Segments = new Collection<Segment>
                    {
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(3),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(4)
                        },
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(4),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(5)
                        }
                    }
                },
                new()
                {
                    Segments = new Collection<Segment>
                    {
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(10),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(11)
                        },
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(12),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(13)
                        }
                    }
                }
            };
            var flightToBeFiltered = new Flight
            {
                Segments = new Collection<Segment>
                {
                    new()
                    {
                        DepartureDate = DateTime.Today - TimeSpan.FromDays(3),
                        ArrivalDate = DateTime.Today
                    },
                    new()
                    {
                        DepartureDate = DateTime.Today - TimeSpan.FromDays(2),
                        ArrivalDate = DateTime.Today + TimeSpan.FromDays(1)
                    }
                }
            };

            #endregion

            flights.Add(flightToBeFiltered);
            var filterResult = flights.Filter(filter);

            Assert.DoesNotContain(flightToBeFiltered, filterResult);
        }

        [Fact]
        public void ArrivalBeforeDepartureFilterTest()
        {
            var filter = new IFlightFilter[] {new ArrivalBeforeDepartureFilter()};

            #region TestVariables

            var flights = new List<Flight>
            {
                new()
                {
                    Segments = new Collection<Segment>
                    {
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(3),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(4)
                        },
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(4),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(5)
                        }
                    }
                },
                new()
                {
                    Segments = new Collection<Segment>
                    {
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(10),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(11)
                        },
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(12),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(13)
                        }
                    }
                }
            };
            var flightToBeFiltered = new Flight
            {
                Segments = new Collection<Segment>
                {
                    new()
                    {
                        DepartureDate = DateTime.Today - TimeSpan.FromDays(2),
                        ArrivalDate = DateTime.Today + TimeSpan.FromDays(1)
                    },
                    new()
                    {
                        DepartureDate = DateTime.Today,
                        ArrivalDate = DateTime.Today - TimeSpan.FromDays(3)
                    }
                }
            };

            #endregion

            flights.Add(flightToBeFiltered);
            var filterResult = flights.Filter(filter);

            Assert.DoesNotContain(flightToBeFiltered, filterResult);
        }

        [Fact]
        public void TwoHoursDelayFilterTest()
        {
            var filter = new IFlightFilter[] {new TwoHoursDelayFilter()};

            #region TestVariables

            var flights = new List<Flight>
            {
                new()
                {
                    Segments = new Collection<Segment>
                    {
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(3),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(4)
                        },
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(4) + TimeSpan.FromMinutes(30),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(5)
                        }
                    }
                },
                new()
                {
                    Segments = new Collection<Segment>
                    {
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(10),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(11)
                        },
                        new()
                        {
                            DepartureDate = DateTime.Today + TimeSpan.FromDays(11) + TimeSpan.FromMinutes(40),
                            ArrivalDate = DateTime.Today + TimeSpan.FromDays(13)
                        }
                    }
                }
            };
            var flightToBeFiltered = new Flight
            {
                Segments = new Collection<Segment>
                {
                    new()
                    {
                        DepartureDate = DateTime.Today - TimeSpan.FromDays(2),
                        ArrivalDate = DateTime.Today + TimeSpan.FromDays(1)
                    },
                    new()
                    {
                        DepartureDate = DateTime.Today + TimeSpan.FromDays(1) + TimeSpan.FromHours(3),
                        ArrivalDate = DateTime.Today - TimeSpan.FromDays(3)
                    }
                }
            };

            #endregion

            flights.Add(flightToBeFiltered);
            var filterResult = flights.Filter(filter);

            Assert.DoesNotContain(flightToBeFiltered, filterResult);
        }
    }
}
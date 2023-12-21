using ConsoleApp1.Interfaces;
using ConsoleApp1.Helpers;

namespace ConsoleApp1.Models
{
    public class NextEventInCityStrategy : INextEventInCityStrategy
    {
        public async IAsyncEnumerable<Event> Execute(IEnumerable<Event> events, Customer customer)
        {
            var filteredEvents = events.Where(e => e.City == customer.City && e.Date > DateTime.Now)
                                       .OrderBy(e => e.Date)
                                       .Take(1);

            foreach (var evt in filteredEvents)
            {
                yield return await Task.FromResult(evt);
            }
        }
    }

    public class ClosestEventToBirthdayStrategy : IClosestEventToBirthdayStrategy
    {
        public async IAsyncEnumerable<Event> Execute(IEnumerable<Event> events, Customer customer)
        {
            var nextBirthday = customer.GetNextBirthday();
            var filteredEvents = events.Where(e => e.Date >= nextBirthday)
                                       .OrderBy(e => Math.Abs((e.Date - nextBirthday).TotalDays))
                                       .Take(1);

            foreach (var evt in filteredEvents)
            {
                yield return await Task.FromResult(evt);
            }
        }
    }

    public class GeographicallyClosestEventsStrategy : IGeographicallyClosestEventsStrategy
    {
        private readonly int _numberOfEvents;

        public GeographicallyClosestEventsStrategy(int numberOfEvents)
        {
            _numberOfEvents = numberOfEvents;
        }

        public async IAsyncEnumerable<Event> Execute(IEnumerable<Event> events, Customer customer)
        {
            var filteredEvents = events.OrderBy(e => GeoHashWrapper.GetDistanceBetweenGeohashes(e.GeoHash, customer.GeoHash))
                                       .Take(_numberOfEvents);

            foreach (var evt in filteredEvents)
            {
                yield return await Task.FromResult(evt);
            }
        }
    }

    public class CheapestTicketsWithinRadiusStrategy : ICheapestTicketsWithinRadiusStrategy
    {
        private readonly double _radius;
        private readonly int _numberOfTickets;

        public CheapestTicketsWithinRadiusStrategy(int numberOfTickets, double radius)
        {
            _numberOfTickets = numberOfTickets;
            _radius = radius;
        }

        public async IAsyncEnumerable<Event> Execute(IEnumerable<Event> events, Customer customer)
        {
            var filteredEvents = events.Where(e => GeoHashWrapper.AreGeohashesNear(e.GeoHash, customer.GeoHash, _radius))
                                       .OrderBy(e => e.TicketPrice)
                                       .Take(_numberOfTickets);

            foreach (var evt in filteredEvents)
            {
                yield return await Task.FromResult(evt);
            }
        }
    }
}

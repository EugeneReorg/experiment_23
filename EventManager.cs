using ConsoleApp1.Interfaces;
using ConsoleApp1.Models;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1.Managers
{
    public class EventManager : IEventManager 
    {
        private SortedDictionary<DateTime, List<Event>> eventsByDate;
        private readonly ILogger<EventManager> _logger;
        private readonly object _lockObj = new object();

        public EventManager(ILogger<EventManager> logger)
        {
            eventsByDate = new SortedDictionary<DateTime, List<Event>>();
            _logger = logger;
        }

        public void AddEvent(Event evt)
        {
            lock (_lockObj)
            {
                if (!eventsByDate.ContainsKey(evt.Date.Date))
                {
                    eventsByDate.Add(evt.Date.Date, new List<Event>());
                }
                eventsByDate[evt.Date.Date].Add(evt);
            }
        }

        // Traditional method for fetching city events
        public IEnumerable<Event> GetNextEventInCity(Customer customer)
        {
            lock (_lockObj)
            {
                return eventsByDate
                    .Where(kv => kv.Key > DateTime.Today)
                    .SelectMany(kv => kv.Value)
                    .Where(e => e.City == customer.City)
                    .OrderBy(e => e.Date)
                    .Take(1)
                    .ToList();
            }
        }

        // Method using strategy pattern with IAsyncEnumerable<T>
        public async IAsyncEnumerable<Event> GetEvents(Customer customer, IEventStrategy strategy)
        {
            List<Event> allEvents;
            lock (_lockObj)
            {
                allEvents = eventsByDate.Values.SelectMany(e => e).ToList();
            }

            await foreach (var evt in strategy.Execute(allEvents, customer))
            {
                yield return evt;
            }
        }

        // Other methods...
    }
}

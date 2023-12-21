using ConsoleApp1.Models;

namespace ConsoleApp1.Interfaces
{
    public interface IEventManager
    {
        void AddEvent(Event evt);
        IEnumerable<Event> GetNextEventInCity(Customer customer);
        IAsyncEnumerable<Event> GetEvents(Customer customer, IEventStrategy strategy);
    }
}

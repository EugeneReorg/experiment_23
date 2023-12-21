using ConsoleApp1.Models;

namespace ConsoleApp1.Interfaces
{
    public interface IEventStrategy
    {
        IAsyncEnumerable<Event> Execute(IEnumerable<Event> events, Customer customer);
    }

    public interface INextEventInCityStrategy : IEventStrategy { }

    public interface IClosestEventToBirthdayStrategy : IEventStrategy { }

    public interface IGeographicallyClosestEventsStrategy : IEventStrategy { }

    public interface ICheapestTicketsWithinRadiusStrategy : IEventStrategy { }

}

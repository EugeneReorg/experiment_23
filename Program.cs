using ConsoleApp1.Interfaces;
using ConsoleApp1.Managers;
using ConsoleApp1.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Setting up Dependency Injection 
        var serviceProvider = ConfigureServices();

        // Resolving the EventManager and strategies
        var eventManager = serviceProvider.GetService<IEventManager>();
        var nextEventInCityStrategy = serviceProvider.GetService<INextEventInCityStrategy>();
        var closestEventToBirthdayStrategy = serviceProvider.GetService<IClosestEventToBirthdayStrategy>();
       // var geographicallyClosestEventsStrategy = serviceProvider.GetService<IGeographicallyClosestEventsStrategy>();
       // var cheapestTicketsWithinRadiusStrategy = serviceProvider.GetService<ICheapestTicketsWithinRadiusStrategy>();

        // Adding sample events and creating a customer
        AddSampleEvents(eventManager);
        var customer = new Customer("John Doe", "New York", new DateTime(1985, 5, 20), (40.7128, -74.0060));

        // Stage 1: Using NextEventInCityStrategy
        var eventsStage1 = eventManager.GetNextEventInCity(customer);
        PrintEvents("Next Event in Customer's City", eventsStage1);

        // Stage 2: Using ClosestEventToBirthdayStrategy
        await foreach (var eventStage2 in eventManager.GetEvents(customer, closestEventToBirthdayStrategy))
        {
            Console.WriteLine($"Event Closest to Birthday: {eventStage2.EventName}");
        }

        // Additional stages using other strategies...
    }

    private static ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            .AddSingleton<IEventManager, EventManager>()
            .AddSingleton<INextEventInCityStrategy, NextEventInCityStrategy>()
            .AddSingleton<IClosestEventToBirthdayStrategy, ClosestEventToBirthdayStrategy>()
            .AddSingleton<IGeographicallyClosestEventsStrategy, GeographicallyClosestEventsStrategy>()
            .AddSingleton<ICheapestTicketsWithinRadiusStrategy, CheapestTicketsWithinRadiusStrategy>()
            .AddLogging(configure => configure.AddConsole())
            .BuildServiceProvider();
    }
    private static void AddSampleEvents(IEventManager eventManager)
    {
        // Define some sample events
        var sampleEvents = new List<Event>
        {
            new Event("Summer Music Festival", "New York", DateTime.Now.AddDays(30), 45.00, (40.7128, -74.0060)),
            new Event("Tech Conference 2023", "San Francisco", DateTime.Now.AddDays(45), 120.00, (37.7749, -122.4194)),
            new Event("Marathon", "Chicago", DateTime.Now.AddDays(60), 30.00, (41.8781, -87.6298)),
            new Event("Art Exhibit", "Boston", DateTime.Now.AddDays(15), 35.00, (42.3601, -71.0589))
        };

        // Add each sample event to the EventManager
        foreach (var evt in sampleEvents)
        {
            eventManager.AddEvent(evt);
        }
    }

    private static void PrintEvents(string title, IEnumerable<Event> events)
    {
        Console.WriteLine($"\n{title}:");
        foreach (var evt in events)
        {
            Console.WriteLine($"- {evt.EventName} on {evt.Date:d} at {evt.City}");
        }
    }
}
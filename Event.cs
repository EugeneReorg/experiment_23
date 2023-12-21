using System;

namespace ConsoleApp1.Models
{
    public class Event
    {
        public string EventName { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public double TicketPrice { get; set; }
        public (double Latitude, double Longitude) Coordinates { get; set; }
        public string GeoHash { get; private set; } // Assuming you are using geohashing

        public Event(string eventName, string city, DateTime date, double ticketPrice, (double Latitude, double Longitude) coordinates)
        {
            EventName = eventName;
            City = city;
            Date = date;
            TicketPrice = ticketPrice;
            Coordinates = coordinates;
            // GeoHash can be set here if you have a method to generate it based on coordinates
            // GeoHash = GenerateGeoHash(Coordinates); 
        }

        // Additional methods or properties, if necessary
    }
}

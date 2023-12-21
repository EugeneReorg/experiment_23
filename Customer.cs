using System;

namespace ConsoleApp1.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime Birthday { get; set; }
        public (double Latitude, double Longitude) Coordinates { get; set; }
        public string GeoHash { get; private set; } // Assuming geohashing is used

        public Customer(string name, string city, DateTime birthday, (double Latitude, double Longitude) coordinates)
        {
            Name = name;
            City = city;
            Birthday = birthday;
            Coordinates = coordinates;
            // Similar to the Event class, GeoHash can be set here
            // GeoHash = GenerateGeoHash(Coordinates);
        }

        public DateTime GetNextBirthday()
        {
            var today = DateTime.Today;
            var nextBirthday = new DateTime(today.Year, Birthday.Month, Birthday.Day);
            if (nextBirthday < today)
            {
                nextBirthday = nextBirthday.AddYears(1);
            }
            return nextBirthday;
        }

        // Additional methods or properties, if necessary
    }
}

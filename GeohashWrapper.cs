using Geohash;  // Assuming 'Geohash' is the namespace for geohash-dotnet
using System;

namespace ConsoleApp1.Helpers
{
    public static class GeoHashWrapper
    {
        public static string Encode(double latitude, double longitude, int precision = 5)
        {
            return new Geohasher().Encode(latitude, longitude, precision);
        }

        public static (double Latitude, double Longitude) Decode(string geohash)
        {
            var coordinates = new Geohasher().Decode(geohash);
            return (coordinates.Item1, coordinates.Item2);
        }

        public static bool AreGeohashesNear(string hash1, string hash2, double radius)
        {
            int prefixLength = GetPrefixLengthBasedOnRadius(radius);
            return hash1.Substring(0, Math.Min(prefixLength, hash1.Length)) ==
                   hash2.Substring(0, Math.Min(prefixLength, hash2.Length));
        }

        private static int GetPrefixLengthBasedOnRadius(double radius)
        {
            if (radius > 1000) return 1;
            if (radius > 500) return 2;
            if (radius > 100) return 3;
            if (radius > 10) return 4;
            return 5;
        }

        public static double GetDistanceBetweenGeohashes(string hash1, string hash2)
        {
            if (!AreGeohashesNear(hash1, hash2, 100)) // Using a fixed radius to check proximity
            {
                return double.MaxValue; // Return a large value indicating 'not near'
            }

            var coord1 = Decode(hash1);
            var coord2 = Decode(hash2);

            return CalculateEuclideanDistance(coord1, coord2);
        }

        private static double CalculateEuclideanDistance((double Latitude, double Longitude) coords1, (double Latitude, double Longitude) coords2)
        {
            double latDistance = coords2.Latitude - coords1.Latitude;
            double lonDistance = coords2.Longitude - coords1.Longitude;
            return Math.Sqrt(latDistance * latDistance + lonDistance * lonDistance);
        }
    }
}

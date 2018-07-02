using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCoonConsole.Models.Geo
{
    class GeoCoordinate

    {
        private double lati;
        public double Lati
        { get; set; }

        private double longi;
        public double Longi
        { get; set; }



        public GeoCoordinate(double lati, double longi)
        {
            this.Lati = lati;
            this.Longi = longi;
        }

        public double calculate_distance(GeoCoordinate other)
        {
            double circumference = 40000.0; // Earth's circumference at the equator in km
            double distance = 0.0;
            double latitude1Rad = DegreesToRadians(this.Lati);
            double latititude2Rad = DegreesToRadians(other.Lati);
            double longitude1Rad = DegreesToRadians(this.Longi);
            double longitude2Rad = DegreesToRadians(other.Lati);
            double logitudeDiff = Math.Abs(longitude1Rad - longitude2Rad);

            if (logitudeDiff > Math.PI)
            {
                logitudeDiff = 2.0 * Math.PI - logitudeDiff;
            }

            double angleCalculation =
                Math.Acos(
                  Math.Sin(latititude2Rad) * Math.Sin(latitude1Rad) +
                  Math.Cos(latititude2Rad) * Math.Cos(latitude1Rad) * Math.Cos(logitudeDiff)
                );

            distance = circumference * angleCalculation / (2.0 * Math.PI);

            return distance;
        }



        private static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

    } // end class
} // end Namespace

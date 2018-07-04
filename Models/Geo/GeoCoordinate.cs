using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AirCoonConsole.Models.Geo
{
    [Serializable]
    public class GeoCoordinate
        : ISerializable
    {
        readonly Decimal Latitude;

        readonly Decimal Longitude;

        readonly int Altitude;


        public GeoCoordinate(Decimal latitude, Decimal longitude, int altitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Altitude = altitude;
        }


        // Deserializer
        public GeoCoordinate(SerializationInfo info, StreamingContext ctxt)
        {
            this.Latitude= info.GetDecimal("Latitude");
            this.Longitude = info.GetDecimal("Longitude");
            this.Altitude = info.GetInt32("Altitude");
        }

        // Serializer
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Latitude", this.Latitude);
            info.AddValue("Longitude", this.Longitude);
            info.AddValue("Altitude", this.Altitude);
        }





            public double calculate_distance(GeoCoordinate other)
        {
            double circumference = 40000.0; // Earth's circumference at the equator in km
            double distance = 0.0;
            double latitude1Rad = DegreesToRadians(this.Latitude);
            double latititude2Rad = DegreesToRadians(other.Latitude);
            double longitude1Rad = DegreesToRadians(this.Longitude);
            double longitude2Rad = DegreesToRadians(other.Latitude);
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



        private static Double DegreesToRadians(Decimal angle) => 
            (Double)(Math.PI * (Double)angle / 180.0);

    } // end class
} // end Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCoonConsole.Handler;

namespace AirCoonConsole.Models
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
                  Math.Cos(latititude2Rad) * Math.Cos(latitude1Rad) * Math.Cos(logitudeDiff));
            distance = circumference * angleCalculation / (2.0 * Math.PI);
            return distance;

        }

        private static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

    }

    public class Continent {

        public static Dictionary<String, Continent> Continents = new Dictionary<String, Continent>();

        public string Code
        {
            get;
        }
        public string Name
        {
            get;
        }
        // Define which month the spring starts (for weather calculation
        public int[] WeatherYear
        {
            get;
        }

        public Continent(string code, string name, int[] weatheryear)
        {
            // check code
            if (code.Length != 2)
            {
                throw new SaveGameException("Continent Code " + code + " not valid");
            }
            this.Code = code;
            this.Name = name;

            // check weather
            foreach (int weather in weatheryear)
            {
                if (weather < 0 || weather > 1000)
                {
                    throw new SaveGameException("Weather for " + code + " not valid: Value: " + weather);
                }
            } // end foreach check weather
            this.WeatherYear = weatheryear;
            //Debug.Write("MEEEEEHHHHHHHHHHHHHHHRFFFACH?" + this.Code, 1);
            Continents.Add(this.Code, this);

        } // end Constructor




    }

    public class Country
    {

        public static Dictionary<String, Country> Countries = new Dictionary<String, Country>();

        // Country Code
        private String code;
        public String Code
        {
            get { return code; }
            set { if (Code.Length == 2) code = Code.ToUpper(); }
        }

        // Country Name
        private String name;
        public String Name { get; set; }

        //Continent
        private Continent cont;
        public Continent Cont { get { return this.cont; } }
        public String ContCode
        {
            get { return cont.Code; }
            set
            {
                if (!Continent.Continents.ContainsKey(ContCode))
                {
                    throw new SaveGameException("Continent does not exist");
                }
                else
                {
                    cont = Continent.Continents[ContCode];
                }
            } // ENd set
        } // End ContCode

        public Country(String code, String name, String continent)
        {
            this.Code = code;
            this.Name = name;
            this.ContCode = continent;

            Countries.Add(this.Code, this);
        }

    }// end Country

    public class Region
    {
        //---- TODO

    } // end class Region

} // End Namespace

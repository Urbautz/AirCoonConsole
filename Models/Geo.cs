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

        public Continent(string code, string name, int[] weatheryear, SaveGame sg, bool SaveToDatabase)
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

            // Database Insert
            if (SaveToDatabase)
            {
                Dictionary<string, string> dbvalues = new Dictionary<string, string>
                {
                    ["code"] = this.Code,
                    ["name"] = this.Name
                };

                int i = 1;
                foreach (int weather in this.WeatherYear)
                {
                    dbvalues.Add("w" + i, weather.ToString());
                    i++;
                }
                Database.SimpleInsert("continent", dbvalues);
            }
            sg.Continents.Add(this.Code, this);

        } // end Constructor




    } // end class Continent

    public class Country 
    {

        public static Dictionary<String, Country> Countries = new Dictionary<String, Country>();

        // Country Code
        public String Code;

        // Country Name
        public String Name;

        //Continent

        public Continent Continent;


        public Country(String code, String name, Continent continent, SaveGame sg, bool SaveToDatabase)
        {
            this.Code = code;
            this.Name = name;
            this.Continent = continent;

            if (SaveToDatabase)
            {
                Dictionary<string, string> dbvalues = new Dictionary<string, string>
                {
                    ["code"] = this.Code,
                    ["name"] = this.Name,
                    ["Continent"] = this.Continent.Code
                };
                Database.SimpleInsert("country", dbvalues);
            }

            sg.Countries.Add(this.Code, this);
        } // End Constructor

    }// end Country

    public class Region
    {
        //---- TODO

    } // end class Region

} // End Namespace

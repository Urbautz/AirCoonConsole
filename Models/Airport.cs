using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCoonConsole.Handler;
using AirCoonConsole.Models.Geo;

namespace AirCoonConsole.Models
{
    class Airport
    {
        // Hashmap Airport-IATA-Code -> Airport-Object, Contains all created Airports
        static Dictionary<String, Airport> Airports;

        private String icao;
        public String Icao
        {
            get { return icao; }
            set
            {
                if (Icao.Length == 4) {
                    icao = Icao.ToUpper();
                } else
                {
                    throw new SaveGameException("Airport ICAO code " + Icao + " not valid");
                }
            } //end set icao
        } // end icao

        private String iata;
        public String Iata
        {
            get { return iata; }
            set
            {
                if (Iata.Length == 3)
                {
                    iata = Iata.ToUpper();
                }
                else
                {
                    throw new SaveGameException("Airport IATA code " + Iata + " not valid");
                }
            } //end set iata
        } // end iata

        private GeoCoordinate coordinate;
        public GeoCoordinate Coordinate { get; set; }
    }

   
}


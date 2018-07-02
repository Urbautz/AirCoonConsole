using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCoonConsole.Handler;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AirCoonConsole.Models.Geo
{
    [Serializable()]
    public class Continent
        : ISerializable
    {

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

            SaveGamePublic.SaveGame.Continents.Add(this.Code, this);

        } // end Constructor4

        //Deserialization constructor.
        public Continent(SerializationInfo info, StreamingContext ctxt)
        {
            //Debug.Write("Continent Deserialisation: " + info.GetString("Code"), 2);
            this.Code = info.GetString("Code");
            this.Name = info.GetString("Name");
            this.WeatherYear = new int[12];
            for (int i = 0; i < WeatherYear.Length; i++)
            {
                this.WeatherYear[i] = info.GetInt32("WeatherYear" + i);
            }
            SaveGamePublic.SaveGame.Continents.Add(this.Code, this);

        } // end Constructur Deserialisation

        // Serialisation
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Code", this.Code);
            info.AddValue("Name", this.Name);
            for(int i = 0; i <= 11; i++)
            {
                info.AddValue("WeatherYear" + i, this.WeatherYear[i]);
            }
         }


    } // ENd class
} // end Namespace

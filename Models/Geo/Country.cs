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
    public class Country
                : ISerializable
    {

        // Country Code
        public readonly String Code;

        // Country Name
        public readonly String Name;

        //Continent
        public readonly Continent Continent;


        public Country(String code, String name, Continent continent)
        {
            this.Code = code;
            this.Name = name;
            this.Continent = continent;


            SaveGamePublic.SaveGame.Countries.Add(this.Code, this);
        } // End Constructor


        //Deserialization constructor
        public Country(SerializationInfo info, StreamingContext ctxt)
        {
            //Debug.Write("Continent Deserialisation: " + info.GetString("Code"), 2);
            this.Code = info.GetString("Code");
            this.Name = info.GetString("Name");
            string Cont = info.GetString("Continent");
            this.Continent = SaveGamePublic.SaveGame.Continents[Cont];

            //SaveGamePublic.SaveGame.Countries.Add(this.Code, this);
        } // end Constructur Deserialisation

        // Serialisation
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Code", this.Code);
            info.AddValue("Name", this.Name);
            info.AddValue("Continent", this.Continent.Code);
        }

    }// end Country
} // end Namespace

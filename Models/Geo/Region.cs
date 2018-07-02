using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using AirCoonConsole.Models;
using AirCoonConsole.Handler;

namespace AirCoonConsole.Models.Geo
{
    [Serializable]
    public class Region
        : ISerializable
    {

        public readonly String Code;
        public readonly String Name;
        public readonly Country Country;

        // constructor
        public Region(String code, string name, Country country)
        {
            this.Code = code;
            this.Name = name;
            this.Country = country;

            SaveGamePublic.SaveGame.Regions.Add(this.Code, this);
        }

        // Deserializer
        public Region(SerializationInfo info, StreamingContext ctxt)
        {
            this.Code = info.GetString("Code");
            this.Name = info.GetString("Name");
            this.Country = SaveGamePublic.SaveGame.Countries[info.GetString("Country")];
        }

        // Serializer
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Code", this.Code);
            info.AddValue("Name", this.Name);
            info.AddValue("Country", this.Country.Code);
        }

    }
}

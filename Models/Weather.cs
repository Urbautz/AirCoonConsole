using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AirCoonConsole.Models
{
    [Serializable()]
    public class Weather
        : ISerializable
    {
        public string foo;

        // Constructor
        public Weather()
        {
            this.foo = "bar";
        }

        //Deserialization constructor
        public Weather(SerializationInfo info, StreamingContext ctxt)
        {
            foo = info.GetString("foo");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("foo", foo);
        }
    }
}

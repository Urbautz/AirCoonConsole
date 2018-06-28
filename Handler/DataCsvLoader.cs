using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AirCoonConsole.Handler
{
    class DataCsvLoader
    {
        private StreamReader Stream;
        private String LastLine;
        private String PrevLine;

        public DataCsvLoader(StreamReader stream, bool stripfirstline=true)
        {
            this.Stream = stream;
            if(stripfirstline)
            {
                LastLine = Stream.ReadLine();
                Debug.Write("CSV Line read", 2);
                
            }
        }

        public String[] getNextLine(int minElements = 2)
        {
            PrevLine = LastLine;
            LastLine = Stream.ReadLine();
            
            if(LastLine == null)
            {
                Stream.Close();
                return null;
            }
            
            String[] splitted = LastLine.Split(new Char[] { ',', ';' });
            if (splitted.Length == 0)
            {
                Debug.Write("Empty line read", 1);
                Stream.Close();
                return null;
            }
             if (splitted[minElements-1] is null )
            {
                Debug.Write("Min. number of elements not found " + PrevLine, 1);
                Stream.Close();
                return null;
            }
            
            return splitted;
            
        }


    }
}

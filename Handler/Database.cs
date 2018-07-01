using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DataAccessLibrary;
using Microsoft.Data.Sqlite;
using System.Data;

namespace AirCoonConsole.Handler
{
    class Database
    {
        private static SqliteConnection conn = null;        


        public static void connect(String savegamefolder)
        {

            if (conn == null)
            {
                string filename = "db.dat";
                string path = 
                    savegamefolder
                    + "\\"
                    + filename
                    + ";"
                    ;

                String conString =
                      "Data Source=" + path;
                    

                Debug.Write("Constring: " + conString, 2); 
                conn = new SqliteConnection(conString);
                
                conn.Open();
            }

         } // end connect   

        public static void CommandQuery(String command, Dictionary<String,String> binds)
        {
            SqliteCommand Command = new SqliteCommand();
            Command.Connection = conn;
            Command.CommandText = command;
            if (binds != null)
            {
                SetBinds(Command, binds);
            }
            Command.ExecuteReader();

        }

        public static void SimpleInsert(String table, Dictionary<String, String> binds)
        {
            if (binds != null && binds.Count() >= 1)
            {

                String qry = "INSERT INTO " + table + " VALUES (";
                int previous = 0;
                foreach (KeyValuePair<string, string> pair in binds)
                {
                    if(previous > 0 && previous < binds.Count())
                    {
                        qry += ", ";
                       
                    }
                    
                    previous++;
                   
                    qry += "@" + pair.Key;
                }
                qry += ");";
                //Debug.Write(qry,3);
                CommandQuery(qry, binds);


            } else
            {
                throw new DataBaseException("Not enough arguments for Insert");
            }
        } // ENd Command Simple Insert
        public static void close()
        {
            conn.Close();
            conn = null;
        }

        private static void SetBinds(SqliteCommand Command, Dictionary<String, String> binds)
        {
            foreach (KeyValuePair<string, string> pair in binds)
            {
                Command.Parameters.AddWithValue("@" + pair.Key, pair.Value);
            }

        }
    } // end class

    public class DataBaseException : Exception
    {
        public DataBaseException(string message)
            : base(message)
        {
        }
    } //end Exception
} // end namespace

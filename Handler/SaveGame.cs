using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using AirCoonConsole.Models;

namespace AirCoonConsole.Handler
{
    public class SaveGame
    {

        private List<String> AllSaveGameNames;
        private String SaveGameFolder;
        private String[] SaveGamePath = new String[] { "My Games", "AirCoon", "saves"};
        private String ConcreteSaveGameFolder;
        private String ConfigPath;


        public Dictionary<String, Continent> Continents = new Dictionary<String, Continent>();
        public Dictionary<String, Country> Countries = new Dictionary<String, Country>();

        private SaveGame()
        {

        }

        /* Return all availibe SaveGames
         */
        public void SetPaths()
        {
            // Config Path
            ConfigPath = Environment.CurrentDirectory;
            ConfigPath += "\\Data";

            // Savegamepath
            this.SaveGameFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            
            AllSaveGameNames = new List<String>();
            SaveGameFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Debug.Write("Savegamefolder: " + SaveGameFolder);

            if (!Directory.Exists(SaveGameFolder))
            {
                throw new System.InvalidOperationException("Savegame-Folder cannot be found");
            }
            foreach (String folder in SaveGamePath)
            {
                SaveGameFolder += "\\" + folder;
                
                if (!Directory.Exists(SaveGameFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(SaveGameFolder);
                    Debug.Write("Savegamefolder had to be created - might be first launch", 3);
                }

            } // End foreach

        } // End set path

        public static List<String> getAvailibleSavegames()
        {
            SaveGame sg = new SaveGame();
            sg.SetPaths();
            

           String[] AllSaveGamePath =  Directory.GetDirectories(sg.SaveGameFolder);
           foreach(String path in AllSaveGamePath)
            {
                sg.AllSaveGameNames.Add(path.Substring(path.Length-3));
            }
           return sg.AllSaveGameNames;

        } // end getavailiblesavegames





        /* This just loads the savegame*/
        public SaveGame(String savegamename) {
            this.load(savegamename);
        } // end constructor




        /* This copies all the files to a new savegamefolder and then loads the savegame */
        public SaveGame(String hub, String code, String name)
        {
            this.SetPaths();
            // Get Config Path
            // Check Code
            // check if name already exists
            Debug.Write("checking airlinecode", 2);
            if(code.Length != 3)
            {
                throw new SaveGameException("Code " + code + " must be 3 digits long");
            }
            code = code.ToUpper();


            // Check letters
            string pattern = @"^[a-zA-Z]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(name))
            {
                throw new SaveGameException("Code " + code + " is not valid.");
            }


            // Check Savegamefolder

            if (Directory.Exists(SaveGameFolder + "\\" + name))
            {
                throw new SaveGameException("Savegame" + code + " already exists.");
            }
            Debug.Write("Check Airlinecode finsihed", 2);


            // Check if valid hub

            // Create directory
            Debug.Write("Creating Directory: " + SaveGameFolder + "\\" + code, 2);
            ConcreteSaveGameFolder = SaveGameFolder + "\\" + code;
            Directory.CreateDirectory(ConcreteSaveGameFolder);

            Debug.Write("Database creation started.",2);
            // Connect to DB
            Database.connect(ConcreteSaveGameFolder);

            // Create Version Table
            Debug.Write("Write Version Information.", 2);
            string qry = "CREATE TABLE IF NOT EXISTS version (version TEXT PRIMARY KEY);";
            Database.CommandQuery(qry, null);

            qry = "INSERT INTO version (version) VALUES (@version);";
            Dictionary<string, string> binds = new Dictionary<string, string>
            {
                { "version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() }
            };
            Database.CommandQuery(qry, binds);

            // Create Continents Table
            Debug.Write("Continents creation", 2);
            qry = "CREATE TABLE IF NOT EXISTS continent ("
                  + "code TEXT PIMARY KEY,"
                  + "name TEXT,";
            for (int i = 1; i<13; i++)
            {
                qry += "weather" + i; 
                if(i<12) qry += ", ";
            }
            qry += ");";
            Database.CommandQuery(qry, null);

            // Load Continents

            Debug.Write("Loading Continents from " + ConfigPath, 3);
            StreamReader stream = new StreamReader(ConfigPath + "\\Continents.dat");
            DataCsvLoader csv = new DataCsvLoader(stream, true);
            List<Continent> continents = new List<Continent>();
            string[] line = csv.getNextLine();
            do
            {
                String contcode = line[0];
                String contname = line[1];
                int[] weather = new int[12];
                for (int i = 2; i < 14; i++)
                {
                    weather[i - 2] = int.Parse(line[i]);
                }
                Continent c = new Continent(contcode, contname, weather, this, true);
                line = csv.getNextLine();
            } while (line != null);
            stream = null;

            
            // Load Countries 
            qry = "CREATE TABLE IF NOT EXISTS country ("
                  + "code TEXT PIMARY KEY,"
                  + "name TEXT,"
                  + "continent TEXT"
                  + ");";
            Database.CommandQuery(qry, null);

            Debug.Write("Loading Countries from " + ConfigPath, 3);
            stream = new StreamReader(ConfigPath + "\\Countries.dat");
            csv = new DataCsvLoader(stream, true);
            line = csv.getNextLine();
            while(line != null)
            {   
                if (!this.Continents.ContainsKey(line[2])) {
                    throw new Exception("Continent not found: " + line[3]);
                }
                Continent cont = this.Continents[line[2]];
                Country country = new Country(line[0], line[1], cont, this, false);
                //Debug.Write("Country created: " + country.Code, 4);
                line = csv.getNextLine();
            }

            //Regionen und Flughäfen laden.

        } // End constructor 

        /* will load an existing savegame, to be called by the Constructors */
        private void load(String savegamename)
        {
            this.SetPaths();
        } // end savegameload


    } // end class

    public class SaveGameException : Exception
    {
        public SaveGameException(string message)
            : base(message)
        {
        }
    } //end Exception


} // end namepsace


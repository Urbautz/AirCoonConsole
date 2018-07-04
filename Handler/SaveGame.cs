using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AirCoonConsole.Models;
using AirCoonConsole.Models.Geo;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AirCoonConsole.Handler
{
    [Serializable]
    public class SaveGame
    {

        private List<String> AllSaveGameNames;
        private String SaveGameFolder;
        private String[] SaveGamePath = new String[] { "My Games", "AirCoon", "saves"};
        private String ConcreteSaveGameFolder;
        private String ConfigPath;

        public Dictionary<String, Continent> Continents = new Dictionary<String, Continent>();
        public Dictionary<String, Country> Countries = new Dictionary<String, Country>();
        public Dictionary<String, Region> Regions = new Dictionary<String, Region>();
        public Dictionary<String, Airport> Airports = new Dictionary<string, Airport>();

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
            //

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

        public static List<String> GetAvailibleSavegames()
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
            SaveGamePublic.SaveGame = this;
            this.Load(savegamename);
            
        } // end constructor




        /* This copies all the files to a new savegamefolder and then loads the savegame */
        public SaveGame(String hub, String code, String name)
        {
            SaveGamePublic.SaveGame = this;
            this.SetPaths();
            // Get Config Path
            // Check Code
            // check if name already exists

            if(code.Length < 2 && code.Length > 3)
            {
                throw new SaveGameException("Code " + code + " must be 2 or 3 digits long");
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


            // Check if valid hub

            // Create directory
            //Debug.Write("Creating Directory: " + SaveGameFolder + "\\" + code, 2);
            ConcreteSaveGameFolder = SaveGameFolder + "\\" + code;
            Directory.CreateDirectory(ConcreteSaveGameFolder);



            // Load Continents

            Debug.Write("Loading Continents", 3);
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
                Continent c = new Continent(contcode, contname, weather);
                line = csv.getNextLine();
            } while (line != null);
            stream = null;


            // Initialize Countries
            Debug.Write("Loading Countries", 3);
            stream = new StreamReader(ConfigPath + "\\Countries.dat");
            csv = new DataCsvLoader(stream, true);
            line = csv.getNextLine();
            while(line != null)
            {   
                if (!this.Continents.ContainsKey(line[2])) {
                    throw new Exception("Continent not found: " + line[3]);
                }
                Continent cont = this.Continents[line[2]];
                Country country = new Country(line[0], line[1], cont);
                line = csv.getNextLine();
            }

            // Initialize Regions
            Debug.Write("Loading Regions", 3);
            stream = new StreamReader(ConfigPath + "\\regions.dat");
            csv = new DataCsvLoader(stream, true);
            line = csv.getNextLine();
            while (line != null)
            {
                if (!this.Countries.ContainsKey(line[1]))
                {
                    throw new Exception("Country not found: " + line[1]);
                }
                Country c = this.Countries[line[1]];
                Region r = new Region(line[0], line[2], c);
                line = csv.getNextLine();
            }

            //Regionen und Flughäfen laden.
            Debug.Write("Loading Airports", 3);

            stream = new StreamReader(ConfigPath + "\\airports.dat");
            csv = new DataCsvLoader(stream, true);
            line = csv.getNextLine();
            while (line != null)
            {
                if (!this.Regions.ContainsKey(line[2]))
                {
                    throw new Exception("Region not found: " + line[2]);
                }
                Region r = this.Regions[line[2]];
                bool Ishub;
                if (line[5] == "1")
                    Ishub = true;
                else Ishub = false;

                GeoCoordinate g;
                short RunwayLength;
                short RunwayCount;
                short slots;
                int passengers;

                try
                {
                    g = new GeoCoordinate(Decimal.Parse(line[6]), Decimal.Parse(line[7]), Int32.Parse(line[8]));
                    RunwayLength = Int16.Parse(line[10]);
                    RunwayCount = Int16.Parse(line[11]);
                    slots = Int16.Parse(line[12]);
                    passengers = Int32.Parse(line[13]);

                } catch (FormatException e)
                {
                    Debug.Write(e.Message);
                    throw new SaveGameException("Airport data corrupted. Airport: " + line[0] + "Not a number:" 
                                + line[6] + ", " + line[8] + ", " + line[8] + ", "+ line[10] + ", "+ line[11] + ", " + line[12] + ", " + line[13]
                                );
                }

                Airport a = new Airport(line[0], line[1], line[4], r, Ishub, g, line[9], line[3], RunwayLength, RunwayCount, slots, passengers);


                line = csv.getNextLine();
            }



            Debug.Write("Load complete", 1);
            this.Save();
            
        } // End constructor 

        /* will load an existing savegame, to be called by the Constructors */
        private void Load(String savegamename)
        {
            this.SetPaths();
        } // end savegameload


        // Saves the game
        public void Save()
        {
            // Inspired by https://www.codeproject.com/Articles/1789/Object-Serialization-using-C
            this.SetPaths();
            BinaryFormatter bformatter = new BinaryFormatter();
            Debug.Write("Saving ...", 1);

            Debug.Write("Saving Continents", 2);
            // Continents
            using (Stream stream = File.Open(this.ConcreteSaveGameFolder + "\\continents.dat", FileMode.Create))
            {
                bformatter.Serialize(stream, this.Continents);
                stream.Close();
            }

            Debug.Write("Saving Countries", 2);
            // Countries
            using (Stream stream = File.Open(this.ConcreteSaveGameFolder + "\\countries.dat", FileMode.Create))
            {
                bformatter.Serialize(stream, this.Countries);
                stream.Close();
            }

            Debug.Write("Saving Regions", 2);
            // Regions
            using (Stream stream = File.Open(this.ConcreteSaveGameFolder + "\\regions.dat", FileMode.Create))
            {
                bformatter.Serialize(stream, this.Regions);
                stream.Close();
            }

            /*
             // Loadtest Region
            this.Regions = null;
            Stream stream2 = File.Open(this.ConcreteSaveGameFolder + "\\regions.dat", FileMode.Open);
            this.Regions = (Dictionary<string, Region>) bformatter.Deserialize(stream2);
            Debug.Write("Regionloader test: " + Regions["AE-FU"].Name);
            Console.ReadLine();
             */

            Debug.Write("Saving Airports", 2);
            using (Stream stream = File.Open(this.ConcreteSaveGameFolder + "\\airports.dat", FileMode.Create))
            {
                bformatter.Serialize(stream, this.Airports);
                stream.Close();
            }

            // Loadtest Region
            this.Airports = null;
            Stream stream2 = File.Open(this.ConcreteSaveGameFolder + "\\airports.dat", FileMode.Open);
            this.Airports = (Dictionary<string, Airport>)bformatter.Deserialize(stream2);
            Debug.Write("Regionloader test: " + this.Airports["FRA"].Size);
            Console.ReadLine();

            Debug.Write("Saved!", 1);
        }

    } // end class

    public class SaveGamePublic
    {
        public static SaveGame SaveGame;
    }

    public class SaveGameException : Exception
    {
        public SaveGameException(string message)
            : base(message)
        {
        }
    } //end Exception


} // end namepsace


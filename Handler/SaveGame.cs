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
    class SaveGame
    {


        private static List<String> AllSaveGameNames;
        private static String SaveGameFolder;
        private static String[] SaveGamePath = new String[] { "My Games", "AirCoon", "saves"};
        private String ConcreteSaveGameFolder;
        private static String ConfigPath;


        /* Return all availibe SaveGames
         */
        public static List<String> getAvailibleSavegames()
        {
            AllSaveGameNames = new List<String>();
            SaveGameFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Debug.Write("Savegamefolder: " + SaveGameFolder);

            if (!Directory.Exists(SaveGameFolder)){
                throw new System.InvalidOperationException("Savegame-Folder cannot be found");
            }
            foreach (String folder in SaveGamePath)
            {
                SaveGameFolder += "\\" + folder;
                Debug.Write("Savegamefolder: " + SaveGameFolder);
                if (!Directory.Exists(SaveGameFolder))
                {
                    DirectoryInfo di = Directory.CreateDirectory(SaveGameFolder);
                    Debug.Write("Savegamefolder had to be created - might be first launch", 1);
                }

            } // End foreach

           String[] AllSaveGamePath =  Directory.GetDirectories(SaveGameFolder);
           foreach(String path in AllSaveGamePath)
            {
                AllSaveGameNames.Add(path.Substring(path.Length-3));
            }
           return AllSaveGameNames;

        } // end getavailiblesavegames


        /* This just loads the savegame*/
        public SaveGame(String savegamename) {
            this.load(savegamename);
        } // end constructor

        /* This copies all the files to a new savegamefolder and then loads the savegame */
        public SaveGame(String hub, String code, String name)
        {
            // Get Config Path
            ConfigPath = Environment.CurrentDirectory;
            ConfigPath += "\\Data";
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




            if (Directory.Exists(SaveGameFolder + "\\" + name))
            {
                throw new SaveGameException("Savegame" + code + " already exists.");
            }
            Debug.Write("Check Airlinecode finsihed", 2);

            // Check if valid hub
            

            
            // Load Continents

            Debug.Write("Loading Continents from " + ConfigPath, 1);
            StreamReader stream = new StreamReader(ConfigPath + "\\Continents.dat");
            DataCsvLoader csv = new DataCsvLoader(stream, true);
            List < Continent > continents = new List<Continent>();
            string[] line = csv.getNextLine();
            do
            {
                Debug.Write("Next line of continent.", 2);
                String contcode = line[0];
                String contname = line[1];
                int[] weather = new int[12];
                  for(int i = 2; i < 14;i++)
                {
                    weather[i - 2] = int.Parse(line[i]);
                    //Debug.Write("weather for month" + (i - 1) + ": " + (weather[i - 2] +1), 3);
                    
                }
                Continent c = new Continent(contcode, contname, weather);
                line = csv.getNextLine();
            } while (line != null);
            stream = null;
            
            hier nun Länder, Regionen und Flughäfen laden.

            // Create directory
            Debug.Write("Creating Directory: " + SaveGameFolder + "\\" + code, 1);
            ConcreteSaveGameFolder = SaveGameFolder + "\\" + code;
            Directory.CreateDirectory(ConcreteSaveGameFolder);


        } // End constructor 
        
        /* will load an existing savegame, to be called by the Constructors */
        private void load(String savegamename)
        {

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


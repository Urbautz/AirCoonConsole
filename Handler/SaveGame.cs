using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCoonConsole.Handler
{
    class SaveGame
    {


        private static List<String> AllSaveGameNames;
        private static String SaveGameFolder;
        private static String[] SaveGamePath = new String[] { "My Games", "AirCoon", "saves"};



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

        }

        /* This just loads the savegame*/
        public SaveGame(String savegamename) {
            this.load(savegamename);
        } // end constructor

        /* This copies all the files to a new savegamefolder and then loads the savegame */
        public SaveGame(String hub, String code, String name)
        {
            // Check Code

            // Check if valid hub

            // Create directory

            // Write files from app and mod folder
            
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


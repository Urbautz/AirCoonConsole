using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCoonConsole.Handler;

namespace AirCoonConsole
{
    class Launcher
    {

        public static string consoleline;
        private static SaveGame savegame;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to AirCoonConsole!");


            // Load config

            // Savegames
            List<String> savegames = SaveGame.getAvailibleSavegames();
            if (savegames.Count == 0)
            {
                Console.WriteLine("No Savegames found.");
            }
            else
            {
                Console.WriteLine("Saves availible:");
                foreach (String SaveGame in savegames)
                {
                    Console.WriteLine(SaveGame);
                }
            } // endelse

            Console.WriteLine("To create a new save just type create ");
            Console.WriteLine("OR type in an airline code to load a saive.");
            consoleline = Console.ReadLine();
            if(consoleline.ToLower() == "create")
            { try
                {
                    savegame = Launcher.NewGame();
                } catch (SaveGameException sge)
                {
                    Debug.Write("Error while creating a new savegame. " + sge.Message);
                } 
            } else
            {
                savegame = Launcher.LoadGame(consoleline);
            }
            Debug.Write("Gameload successfull!");

           

            Debug.Write("Here comes the tickstarter ...",1);
            System.Threading.Thread.Sleep(5000);

        } // end method

        public static SaveGame LoadGame(String gamename)
        {
            Debug.Write("Will load" + gamename + "...",1);
            
            return new SaveGame(gamename);
        }

        public static SaveGame NewGame()
        {
            Debug.Write("Will create new game", 1);

            Console.WriteLine("Please enter the Hub-Code you want to start your airline, then with a blank the 3 digit code and the name");
            Console.WriteLine("Example:");
            Console.WriteLine("JFK TCO Tycoon Airlines");
            consoleline = Console.ReadLine();

            String[] consolewords = consoleline.Split(' ');
            String hub = consolewords[0];
            String code = consolewords[1];
            String name = consolewords[2];
            for(int i=3;i < consolewords.Length;i++)
            {
                name += consolewords[i];
            }
            Debug.Write("Will create airline " + code + " (" + name + ") at " + hub, 2);

            return new SaveGame(hub,code,name);
        }
    } // endclass
} //endnamespace

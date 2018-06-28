using System;
using System.IO;

public static class Debug
{
    private static int DebugLevel = 2;
    //Sleeptimes per Debug-Level
    private static int DebugSleepTime0 = 5000;
    private static int DebugSleepTime1 = 1000;
    private static int DebugSleepTime2 = 500;
    private static int DebugSleepTime3 = 1;
    private static String DebugSeparator0 = "_";
    private static String DebugSeparator1 = "__";
    private static String DebugSeparator2 = "___";
    private static String DebugSeparator3 = "____";


    public static void Write(string text, int level = 2)
    {


            switch (level)
            {
                case 0:
                    Console.WriteLine(DebugSeparator0 + level + "___ " + text);
                    System.Threading.Thread.Sleep(DebugSleepTime0);
                    break;
                case 1:
                    Console.WriteLine(DebugSeparator1 + level + "___ " + text);
                    System.Threading.Thread.Sleep(DebugSleepTime1);
                    break;
                case 2:
                    Console.WriteLine(DebugSeparator2 + level + "___ " + text);
                    System.Threading.Thread.Sleep(DebugSleepTime2);
                    break;
                case 3:
                    Console.WriteLine(DebugSeparator3 + level + "___ " + text);
                    System.Threading.Thread.Sleep(DebugSleepTime3);
                    break;
                default:
                    Console.WriteLine(DebugSeparator0 + level + "___ " + text);
                    break;
         
            }// ENd switch
    }


}
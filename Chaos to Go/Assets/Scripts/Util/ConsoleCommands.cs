using System;
using System.Reflection;
using UnityEngine;


public class ConsoleCommands
{
    private static int argc;
    private static string[] args;
    private static GameConsoleHandler gameConsole;


    public static void Execute(int argc, string[] args, ref string output, GameConsoleHandler gameConsole)
    {
        ConsoleCommands.argc = argc;
        ConsoleCommands.args = args;
        ConsoleCommands.gameConsole = gameConsole;

        ConsoleCommands instance = new ConsoleCommands(); // If only we would be able to program in a functional programming language!
        Type thisType = instance.GetType();
        MethodInfo theMethod = thisType.GetMethod(args[0]);
        if(theMethod == null || args[0].Equals("Execute"))
        {
            foreach(string token in args)
            {
                output += token;
            }
            output += "\n ERROR: Unable to find command '" + args[0] + "'!";
            return;
        }
        output = theMethod.Invoke(instance, null) as string;
    }


    private ConsoleCommands(){}


    private static string BuildOutput()
    {
        string output = "";
        foreach (string token in args)
        {
            output += token + " ";
        }
        return output;
    }



    // ================================================================================================= //
    //                                     BEGIN CONSOLE COMMANDS                                        //
    // ================================================================================================= //

    public static string foo()
    {
        return BuildOutput();
    }


    public static string clear()
    {
        gameConsole.Clear();
        return BuildOutput();
    }


    public static string echo()
    {
        string output = BuildOutput() + "\n";
        for(int i = 1; i < argc; i++)
        {
            output += " " + args[i];
        }
        return output;
    }

    // ================================================================================================= //
    //                                     END CONSOLE COMMANDS                                          //
    // ================================================================================================= //
}

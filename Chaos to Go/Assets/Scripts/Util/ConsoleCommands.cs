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


    public static string exit()
    {
        Application.Quit();
        Debug.Log("Closing Game!");
        return BuildOutput();
    }


    public static string give()
    {
        string output = BuildOutput();
        if (argc < 2)
            return output + "\nPlease specify what you want!\n" +
                            "give <obj> <type> <typeargs> ; <obj> in {tile}";

        switch (args[1])
        {
            case "tile":
                {

                    if (argc < 3)
                    {
                        return output + "\nPlease specify what you want!\n" +
                                        "give tile <tilename>";
                    }

                    switch (args[2])
                    {
                        case "basetile":
                            {
                                if (argc < 5 || args[3] == args[4])
                                {
                                    return output + "\nPlease specify a direction!\n" +
                                                    "give tile basetile <dir1> <dir2> ; <dir1>, <dir2> in {u,d,l,r}";
                                }

                                BaseTile.eDirection start = BaseTile.eDirection.up, end = BaseTile.eDirection.down;
                                switch (args[3])
                                {
                                    case "u":
                                        start = BaseTile.eDirection.up;
                                        break;
                                    case "d":
                                        start = BaseTile.eDirection.down;
                                        break;
                                    case "l":
                                        start = BaseTile.eDirection.left;
                                        break;
                                    case "r":
                                        start = BaseTile.eDirection.right;
                                        break;
                                }
                                switch (args[4])
                                {
                                    case "u":
                                        end = BaseTile.eDirection.up;
                                        break;
                                    case "d":
                                        end = BaseTile.eDirection.down;
                                        break;
                                    case "l":
                                        end = BaseTile.eDirection.left;
                                        break;
                                    case "r":
                                        end = BaseTile.eDirection.right;
                                        break;
                                }

                                TileSelectionMenu menu = GameObject.Find("TileSelectionMenu").GetComponent<TileSelectionMenu>();
                                menu.AddBaseTile(start, end);
                            }
                            break;
                    }

                }
                break;
        }

        return BuildOutput();
    }

    // ================================================================================================= //
    //                                     END CONSOLE COMMANDS                                          //
    // ================================================================================================= //
}

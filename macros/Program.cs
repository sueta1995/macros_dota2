using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace macros
{
    class Program
    {
        static bool IS_STOP = false;
        static string APP_NAME = "Macros Dota2";
        static string APP_VERSION = " beta v0.1.1";
        static Process process = new Process();
        static string FILE_NAME_CONFIG = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\macros_dota2.properties";

        static void Main(string[] args)
        {
            string[] str = Directory.GetFiles(Directory.GetCurrentDirectory());

            Console.Title = APP_NAME;
            Console.WriteLine(">> " + APP_NAME + APP_VERSION);
            Console.WriteLine(">> Write -help to get list of commands");

            try
            {
                CLEAR_FILE(File.ReadAllLines(FILE_NAME_CONFIG));
            }

            catch (Exception e)
            {
                Console.WriteLine(">> Creating file macros_dota2.properties!");
                File.Create(FILE_NAME_CONFIG);
            }

            CONSOLE();

            /*string[] ALL_SHORT_CUT = File.ReadAllLines(FILE_NAME_CONFIG);

            ScriptEngine engine = Python.CreateEngine();
            string dir = Directory.GetCurrentDirectory() + "\\python";
            ICollection<string> paths = engine.GetSearchPaths(); 
            if (!String.IsNullOrWhiteSpace(dir)) paths.Add(dir); 
            else paths.Add(Environment.CurrentDirectory); 
            engine.SetSearchPaths(paths);

            engine.Execute("" +
                "import keyboard");
            */
        }

        static void CONSOLE()
        {
            while (!IS_STOP)
            {
                Console.Write(">> ");
                string CMD = Console.ReadLine();

                if (CMD.ToLower() == "-help")
                {
                    Console.WriteLine(">> Available commands:\n>> -create - create new shortcut\n>> -delete [name] - delete current shortcut\n>> -edit [name] - edit current shortcut\n>> -get - get profiles info\n>> -start - start shortcuts");
                }

                else if (CMD.ToLower() == "-create")
                {
                    bool IS_STOP_CMD = false;
                    string SHORT_CUT = "";

                    Console.Write(">> Write name of profile: ");
                    string PROFILE_NAME = Console.ReadLine();
                    if (PROFILE_NAME.IndexOf(' ') != -1) { Console.WriteLine(">> Incorrect symbols in name"); CONSOLE(); };

                    Console.Write(">> Write hotkey: ");
                    string HOT_KEY = Console.ReadKey().Key.ToString();
                    if (HOT_KEY == "Enter") { Console.WriteLine(">> Incorrect symbol!"); Console.WriteLine(); CONSOLE(); };
                    Console.Write("(" + HOT_KEY + ")");
                    Console.WriteLine();

                    Console.Write(">> Write shortcut, type Enter to stop...\n>> ");
                    while (!IS_STOP_CMD)
                    {
                        string SYMBOL = Console.ReadKey().Key.ToString();

                        if (SYMBOL == "Enter") { IS_STOP_CMD = true; Console.WriteLine(); }
                        else { SHORT_CUT += SYMBOL + " "; Console.Write("(" + SYMBOL + ") "); };
                    }

                    File.AppendAllText(FILE_NAME_CONFIG, PROFILE_NAME + " " + HOT_KEY + " " + SHORT_CUT + "\n");

                    Console.WriteLine(">> Values have been saved!");
                }

                else if (CMD.ToLower().Split(' ')[0] == "-delete" && CMD.Split(' ').Length == 2)
                {
                    try
                    {
                        string[] ALL_SHORT_CUT = File.ReadAllLines(FILE_NAME_CONFIG);
                        bool IS_FOUND = false;

                        for (int i = 0; i < ALL_SHORT_CUT.Length; i++)
                        {
                            if (ALL_SHORT_CUT[i].Split(' ')[0] == CMD.Split(' ')[1])
                            {
                                Array.Clear(ALL_SHORT_CUT, i, 1);
                                IS_FOUND = true;
                            }
                        }

                        if (IS_FOUND) Console.WriteLine(">> Choosen shortcut has been deleted!");
                        else Console.WriteLine(">> This shortcut was not found.");

                        File.WriteAllLines(FILE_NAME_CONFIG, ALL_SHORT_CUT);
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine(">> An occured error: " + e.Message);
                        CONSOLE();
                    }
                }

                else if (CMD.ToLower().Split(' ')[0] == "-edit" && CMD.ToLower().Split(' ').Length == 2)
                {
                    try
                    {
                        string[] ALL_SHORT_CUT = File.ReadAllLines(FILE_NAME_CONFIG);
                        string SHORT_CUT = "";
                        bool IS_FOUND = false;
                        bool IS_STOP_CMD = false;

                        for (int i = 0; i < ALL_SHORT_CUT.Length; i++)
                        {
                            if (ALL_SHORT_CUT[i].Split(' ')[0] == CMD.Split(' ')[1])
                            {
                                Console.Write(">> Write hotkey: ");
                                string HOT_KEY = Console.ReadKey().Key.ToString();
                                if (HOT_KEY == "Enter") { Console.WriteLine(">> Incorrect symbol!"); Console.WriteLine(); CONSOLE(); };
                                Console.Write("(" + HOT_KEY + ")");
                                Console.WriteLine();

                                Console.Write(">> Write shortcut, type Enter to stop...\n>> ");
                                while (!IS_STOP_CMD)
                                {
                                    string SYMBOL = Console.ReadKey().Key.ToString();

                                    if (SYMBOL == "Enter") { IS_STOP_CMD = true; Console.WriteLine(); }
                                    else { SHORT_CUT += SYMBOL + " "; Console.Write("(" + SYMBOL + ") "); };
                                }

                                ALL_SHORT_CUT[i] = ALL_SHORT_CUT[i].Split(' ')[0] + " " + HOT_KEY + " " + SHORT_CUT;

                                IS_FOUND = true;

                            }
                        }

                        if (IS_FOUND) Console.WriteLine(">> Choosen shortcut has been edited!");
                        else Console.WriteLine(">> This shortcut was not found.");

                        File.WriteAllLines(FILE_NAME_CONFIG, ALL_SHORT_CUT);
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine(">> An occured error: " + e.Message);
                        CONSOLE();
                    }
                }

                else if (CMD.ToLower() == "-get")
                {
                    try
                    {
                        string[] ALL_SHORT_CUT = File.ReadAllLines(FILE_NAME_CONFIG);

                        for (int i = 0; i < ALL_SHORT_CUT.Length; i++)
                        {
                            Console.WriteLine("   Name: " + ALL_SHORT_CUT[i].Split(' ')[0]);
                            Console.WriteLine("   Hotkey: " + ALL_SHORT_CUT[i].Split(' ')[1]);
                            Console.WriteLine("   Shortcut: " + ALL_SHORT_CUT[i].Replace(ALL_SHORT_CUT[i].Split(' ')[0] + " " + ALL_SHORT_CUT[i].Split(' ')[1] + " ", ""));
                            Console.WriteLine("");
                        }
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine(">> Incorrect format of string: " + e.Message);
                        CONSOLE();
                    }
                }

                else if (CMD.ToLower() == "-start")
                {
                    CLEAR_FILE(File.ReadAllLines(FILE_NAME_CONFIG));

                    IS_STOP = true;
                    Console.WriteLine(">> Process has been started!");

                    process.StartInfo.FileName = "script.exe";
                    process.Start();
                }

                else
                {
                    Console.WriteLine(">> Incorrect command!");
                }

                try
                {
                    CLEAR_FILE(File.ReadAllLines(FILE_NAME_CONFIG));
                }

                catch (Exception e)
                {
                    Console.WriteLine(">> An error occured: " + e.Message);
                }

            }
        }

        static void CLEAR_FILE(string[] FILE)
        {
            try
            {
                for (int i = 0; i < FILE.Length; i++)
                {
                    if (FILE[i][FILE[i].Length - 1] == ' ')
                        FILE[i] = FILE[i].Remove(FILE[i].Length - 1);
                }
            }
            catch { }

            FILE = FILE.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            File.WriteAllLines(FILE_NAME_CONFIG, FILE);
        }

    }
}
using System;
using System.IO;

using DirectShare.Server;
using DirectShare.Client;

namespace DirectShare
{
    /// <summary>
    /// Main class.
    /// </summary>
    class MainClass
    {
        private static DSClient client;
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            if (args.Length <= 0)
                displayHelp();

            switch (args[0])
            {
                case "-s":
                case "--server":
                    new ServerUI(args[1], Convert.ToInt32(args[2])).RunConsole();
                    break;
                case "-c":
                case "--connect":
                    RunClient(args[1], Convert.ToInt32(args[2]));
                    break;
                default:
                    displayHelp();
                    break;
            }
        }

        private static void displayHelp()
        {
            Console.WriteLine("DirectShare.exe [FLAG] [IP] [PORT]");
            Console.WriteLine("-c --connect [IP] [PORT]\tConnects to a DirectShare server.");
            Console.WriteLine("-h --help\tDisplays this help and exits.");
            Console.WriteLine("-s --server [IP] [PORT]\tStarts a DirectShare server.");

            Environment.Exit(0);
        }

        private static void RunClient(string ip, int port)
        {
            client = new DSClient(ip, port);
            client.DataRecieved += client_OnDataRecieved;
        }

        private static void client_OnDataRecieved(object sender, DataRecievedEventArgs e)
        {
            Console.WriteLine("Data of size " + e.DataSize + " recieved! Accept? y/n ");
            switch (Console.ReadLine().ToLower())
            {
                case "y":
                case "yes":
                    Console.WriteLine("Enter path to save location: ");
                    e.Save(Console.ReadLine());
                    break;
                case "n":
                case "no":
                    break;
                default:
                    client_OnDataRecieved(sender, e);
                    break;
            }
        }
    }
}

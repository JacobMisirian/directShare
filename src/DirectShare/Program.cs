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
        private static DSServer server;
        private static DSClient client;
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            switch (args[0])
            {
                case "-s":
                case "--server":
                    RunServer(args[1], Convert.ToInt32(args[2]));
                    break;
                case "-c":
                case "--client":
                    RunClient(args[1], Convert.ToInt32(args[2]));
                    break;
            }
        }

        private static void RunServer(string ip, int port)
        {
            server = new DSServer(ip, port);
            server.ClientConnected += server_OnClientConnected;
            server.TextRecieved += server_OnTextRecieved;
        }

        private static void server_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("Client connected from " + e.ConnectingClient.IP + " accept? y/n ");
            switch (Console.ReadLine().ToLower())
            {
                case "y":
                case "yes":
                    server.AcceptedClients.Add(e.ConnectingClient);
                    break;
                case "n":
                case "no":
                    break;
                default:
                    server_OnClientConnected(sender, e);
                    break;
            }

            Console.WriteLine("Path to file to send: ");
            e.ConnectingClient.Send(File.ReadAllBytes(Console.ReadLine()));
        }

        private static void server_OnTextRecieved(object sender, TextRecievedEventArgs e)
        {
            Console.WriteLine(e.Message);
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

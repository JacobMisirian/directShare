using System;
using System.IO;

namespace DirectShare.Server
{
    public class ServerUI
    {
        private DSServer server;
        private int highestID = 0;

        public ServerUI(string ip, int port)
        {
            server = new DSServer(ip, port);

            server.ClientConnected += server_OnClientConnected;
            server.TextRecieved += server_OnTextRecieved;
        }

        public void RunConsole()
        {
            while (true)
            {
                Console.Write("> ");
                handleCommand(Console.ReadLine());
            }
        }

        private void handleCommand(string command)
        {
            string[] parts = command.Split(' ');
            switch (parts[0].ToLower())
            {
                case "help":
                    displayHelp();
                    break;
                case "list":
                    foreach (ConnectingClient client in server.ConnectedClients)
                        Console.WriteLine(client.ID + ": " + client.IP);
                    break;
                case "listaccepted":
                    foreach (ConnectingClient client in server.AcceptedClients)
                        Console.WriteLine(client.ID + ": " + client.IP);
                    break;
                case "accept":
                    if (parts.Length <= 1)
                        syntaxError();
                    else
                    {
                        server.AcceptedClients.Add(idToClient(Convert.ToInt32(parts[1])));
                        Console.WriteLine("Client accepted!");
                    }
                    break;
                case "unaccept":
                    if (parts.Length <= 1)
                        syntaxError();
                    else
                    {
                        server.AcceptedClients.Remove(idToClient(Convert.ToInt32(parts[1])));
                        Console.WriteLine("Client unaccepted!");
                    }
                    break;
                case "send":
                    if (parts.Length <= 2)
                        syntaxError();
                    else
                    {
                        switch (parts[1].ToUpper())
                        {
                            case "ACCEPTED":
                                foreach (ConnectingClient client in server.AcceptedClients)
                                    client.Send(parts[2]);
                                break;
                            case "ALL":
                                foreach (ConnectingClient client in server.AcceptedClients)
                                    client.Send(parts[2]);
                                break;
                            default:
                                idToClient(Convert.ToInt32(parts[1])).Send(parts[2]);
                                break;
                        }
                    }
                    break;
            }
        }

        private ConnectingClient idToClient(int id)
        {
            foreach (ConnectingClient client in server.ConnectedClients)
                if (client.ID == id)
                    return client;
            return null;
        }

        private void displayHelp()
        {
            Console.WriteLine("Command:\tDescription:");
            Console.WriteLine("list\tLists all clients IPs and their ID.");
            Console.WriteLine("listAccepted\tLists all the clients that are accepted.");
            Console.WriteLine("accept [ID]\tAdds a client ID to the accepted list.");
            Console.WriteLine("unaccept [ID]\tRemoves a client from the accepted list.");
            Console.WriteLine("send [[ID]/ACCEPTED/ALL] [PATH]\tSends the file at [PATH] to either the ID, accepted list, or all.");
            Console.WriteLine("help\tDisplays this help.");
        }

        private void syntaxError(string details = "")
        {
            Console.WriteLine("Syntax Error! " + details);
        }

        private void server_OnClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Console.WriteLine("Client connected from " + e.ConnectingClient.IP + ". Check client list with list command");
            e.ConnectingClient.ID = highestID++;
        }

        private static void server_OnTextRecieved(object sender, TextRecievedEventArgs e)
        {
            //Console.WriteLine(e.Message);
        }
    }
}


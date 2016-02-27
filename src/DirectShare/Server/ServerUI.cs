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
            server.ClientDisconnected += server_OnClientDisconnected;
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
            try
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
                                    server.SendToAcceptedClients(parts[2]);
                                    break;
                                case "ALL":
                                    server.SendToConnectedClients(parts[2]);
                                    break;
                                default:
                                    server.SendToClient(idToClient(Convert.ToInt32(parts[1])), parts[2]);
                                    break;
                            }
                        }
                        break;
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("No such ID!");
            }
            catch (IndexOutOfRangeException ex)
            {
                syntaxError("Arguments were not correct!");
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
            e.ConnectingClient.ID = highestID++;
            Console.WriteLine("Client connected from " + e.ConnectingClient.IP + " with ID " + e.ConnectingClient.ID + ". Check client list with list command");
        }

        private void server_OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            try
            {
                Console.WriteLine("Client " + e.Client.ID + " disconnected!");
                server.ConnectedClients.Remove(e.Client);
                e.Client.TcpClient.Close();
                if (server.AcceptedClients.Contains(e.Client))
                    server.AcceptedClients.Remove(e.Client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void server_OnTextRecieved(object sender, TextRecievedEventArgs e)
        {
            //Console.WriteLine(e.Message);
        }
    }
}


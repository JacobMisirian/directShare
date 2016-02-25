using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DirectShare.Server
{
    /// <summary>
    /// DirectShare server.
    /// </summary>
    public class DSServer
    {
        /// <summary>
        /// The connected clients.
        /// </summary>
        public List<ConnectingClient> ConnectedClients;
        /// <summary>
        /// The accepted clients.
        /// </summary>
        public List<ConnectingClient> AcceptedClients;

        private TcpListener listener;
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectShare.Server.DSServer"/> class.
        /// </summary>
        public DSServer() {}
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectShare.Server.DSServer"/> class.
        /// </summary>
        /// <param name="ip">Ip.</param>
        /// <param name="port">Port.</param>
        public DSServer(string ip, int port)
        {
            Start(ip, port);
        }
        /// <summary>
        /// Start the specified ip and port.
        /// </summary>
        /// <param name="ip">Ip.</param>
        /// <param name="port">Port.</param>
        public void Start(string ip, int port)
        {
            ConnectedClients = new List<ConnectingClient>();
            AcceptedClients = new List<ConnectingClient>();

            listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();

            new Thread(() => listenForConnections()).Start();
        }
        /// <summary>
        /// Sends to connected clients.
        /// </summary>
        /// <param name="data">Data.</param>
        public void SendToConnectedClients(string data)
        {
            SendToConnectedClients(Encoding.ASCII.GetBytes(data));
        }
        /// <summary>
        /// Sends to connected clients.
        /// </summary>
        /// <param name="data">Data.</param>
        public void SendToConnectedClients(byte[] data)
        {
            foreach (ConnectingClient client in ConnectedClients)
                client.Send(data);
        }
        /// <summary>
        /// Sends to accepted clients.
        /// </summary>
        /// <param name="data">Data.</param>
        public void SendToAcceptedClients(string data)
        {
            SendToAcceptedClients(Encoding.ASCII.GetBytes(data));
        }
        /// <summary>
        /// Sends to accepted clients.
        /// </summary>
        /// <param name="data">Data.</param>
        public void SendToAcceptedClients(byte[] data)
        {
            foreach (ConnectingClient client in AcceptedClients)
                client.Send(data);
        }
        /// <summary>
        /// Accepts the client.
        /// </summary>
        /// <param name="client">Client.</param>
        public void AcceptClient(ConnectingClient client)
        {
            AcceptedClients.Add(client);
        }

        private void listenForConnections()
        {
            while (true)
            {
                ConnectingClient client = new ConnectingClient(listener.AcceptTcpClient());
                client.RecieveThread = new Thread(() => listenForMessages(client));
                client.RecieveThread.Start();

                ConnectedClients.Add(client);
                OnClientConnected(new ClientConnectedEventArgs { ConnectingClient = client });
            }
        }

        private void listenForMessages(ConnectingClient client)
        {
            while (true)
                OnTextRecieved(new TextRecievedEventArgs { Client = client, Message = client.Input.ReadString() });
        }
        /// <summary>
        /// Occurs when client connected.
        /// </summary>
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        protected virtual void OnClientConnected(ClientConnectedEventArgs e)
        {
            EventHandler<ClientConnectedEventArgs> handler = ClientConnected;
            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// Occurs when text recieved.
        /// </summary>
        public event EventHandler<TextRecievedEventArgs> TextRecieved;
        protected virtual void OnTextRecieved(TextRecievedEventArgs e)
        {
            EventHandler<TextRecievedEventArgs> handler = TextRecieved;
            if (handler != null)
                handler(this, e);
        }
    }
}
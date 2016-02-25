using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace DirectShare.Server
{
    /// <summary>
    /// Connecting client.
    /// </summary>
    public class ConnectingClient
    {
        /// <summary>
        /// Gets the tcp client.
        /// </summary>
        /// <value>The tcp client.</value>
        public TcpClient TcpClient { get; private set; }
        /// <summary>
        /// Gets the IP.
        /// </summary>
        /// <value>The IP.</value>
        public string IP { get { return ((IPEndPoint)TcpClient.Client.RemoteEndPoint).Address.ToString(); } }
        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <value>The output.</value>
        public BinaryWriter Output { get; private set; }
        /// <summary>
        /// Gets the input.
        /// </summary>
        /// <value>The input.</value>
        public BinaryReader Input { get; private set; }
        /// <summary>
        /// Gets or sets the recieve thread.
        /// </summary>
        /// <value>The recieve thread.</value>
        public Thread RecieveThread { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectShare.Server.ConnectingClient"/> class.
        /// </summary>
        /// <param name="client">Client.</param>
        public ConnectingClient(TcpClient client)
        {
            TcpClient = client;
            Output = new BinaryWriter(client.GetStream());
            Input = new BinaryReader(client.GetStream());
        }
        /// <summary>
        /// Send the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void Send(string data)
        {
            Send(Encoding.ASCII.GetBytes(data));
        }
        /// <summary>
        /// Send the specified data.
        /// </summary>
        /// <param name="data">Data.</param>
        public void Send(byte[] data)
        {
            Output.Write(data.Length + "\r\n");
            for (int i = 0; i < data.Length; i++)
                Output.Write(data[i]);
            Output.Flush();
        }
    }
}


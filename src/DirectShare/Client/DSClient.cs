using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DirectShare.Client
{
    /// <summary>
    /// DirectShare client.
    /// </summary>
    public class DSClient
    {
        private TcpClient client;
        private BinaryWriter output;
        private BinaryReader input;
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectShare.Client.DSClient"/> class.
        /// </summary>
        public DSClient() {}
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectShare.Client.DSClient"/> class.
        /// </summary>
        /// <param name="ip">Ip.</param>
        /// <param name="port">Port.</param>
        public DSClient(string ip, int port)
        {
            Connect(ip, port);
        }
        /// <summary>
        /// Connect the specified ip and port.
        /// </summary>
        /// <param name="ip">Ip.</param>
        /// <param name="port">Port.</param>
        public void Connect(string ip, int port)
        {
            client = new TcpClient(ip, port);
            output = new BinaryWriter(client.GetStream());
            input = new BinaryReader(client.GetStream());

            new Thread(() => listenForData()).Start();
        }

        private void listenForData()
        {
            while (true)
            {
                double size = Convert.ToDouble(input.ReadString());
                OnDataRecieved(new DataRecievedEventArgs { Reader = input, DataSize = size });
            }
        }
        /// <summary>
        /// Occurs when data recieved.
        /// </summary>
        public event EventHandler<DataRecievedEventArgs> DataRecieved;
        protected virtual void OnDataRecieved(DataRecievedEventArgs e)
        {
            EventHandler<DataRecievedEventArgs> handler = DataRecieved;
            if (handler != null)
                handler(this, e);
        }
    }
}


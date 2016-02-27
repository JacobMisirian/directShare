using System;

namespace DirectShare.Server
{
    public class ClientDisconnectedEventArgs : EventArgs
    {
        public ConnectingClient Client { get; set; }
    }
}


using System;

namespace DirectShare.Server
{
    /// <summary>
    /// Client disconnected event arguments.
    /// </summary>
    public class ClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public ConnectingClient Client { get; set; }
    }
}


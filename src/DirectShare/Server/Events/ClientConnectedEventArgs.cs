using System;

namespace DirectShare.Server
{
    /// <summary>
    /// Client connected event arguments.
    /// </summary>
    public class ClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the connecting client.
        /// </summary>
        /// <value>The connecting client.</value>
        public ConnectingClient ConnectingClient { get; set; }
    }
}


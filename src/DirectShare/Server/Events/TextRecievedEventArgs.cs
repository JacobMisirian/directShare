using System;

namespace DirectShare.Server
{
    /// <summary>
    /// Text recieved event arguments.
    /// </summary>
    public class TextRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public ConnectingClient Client { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
        /// <summary>
        /// Gets the length of the message.
        /// </summary>
        /// <value>The length of the message.</value>
        public int MessageLength { get { return Message.Length; } }
    }
}


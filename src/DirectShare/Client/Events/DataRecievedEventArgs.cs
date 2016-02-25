using System;
using System.IO;
using System.Text;

namespace DirectShare
{
    /// <summary>
    /// Data recieved event arguments.
    /// </summary>
    public class DataRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the reader.
        /// </summary>
        /// <value>The reader.</value>
        public BinaryReader Reader { get; set; }
        /// <summary>
        /// Gets or sets the size of the data.
        /// </summary>
        /// <value>The size of the data.</value>
        public double DataSize { get; set; }
        /// <summary>
        /// Save the specified path.
        /// </summary>
        /// <param name="path">Path.</param>
        public void Save(string path)
        {
            BinaryWriter writer = new BinaryWriter(new StreamWriter(path).BaseStream);
            for (int i = 0; i < DataSize; i++)
                writer.Write(Reader.ReadByte());
            writer.Flush();
            writer.Close();
        }
    }
}


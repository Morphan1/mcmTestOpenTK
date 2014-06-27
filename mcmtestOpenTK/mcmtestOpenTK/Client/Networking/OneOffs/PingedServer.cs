using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.Networking.OneOffs
{
    public class PingedServer
    {
        /// <summary>
        /// The name of the server.
        /// </summary>
        public string Name;

        /// <summary>
        /// The server's IP address.
        /// </summary>
        public string Address;

        /// <summary>
        /// The server's connection port.
        /// </summary>
        public int Port;

        /// <summary>
        /// The server's ping time.
        /// </summary>
        public int Ping;

        public PingedServer(string _name, int _ping, string _address, int _port)
        {
            Name = Utilities.CleanStringInput(_name.Replace("\n", "").Replace("\r", ""));
            Ping = _ping;
            Address = _address.ToLower();
            Port = _port;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace mcmtestOpenTK.Client.Networking
{
    public class NetworkUtil
    {
        /// <summary>
        /// Creates a generic socket object on the appropriate TCP setup.
        /// </summary>
        /// <returns>A valid socket object</returns>
        public static Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}

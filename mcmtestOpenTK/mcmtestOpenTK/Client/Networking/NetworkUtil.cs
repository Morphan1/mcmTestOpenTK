using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using mcmtestOpenTK.Shared;

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

        /// <summary>
        /// Creates a generic socket object on the appropriate TCP setup, for IPv6
        /// </summary>
        /// <returns>A valid socket object</returns>
        public static Socket Createv6Socket()
        {
            return new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Converts arbitrary input text to an IP address.
        /// </summary>
        /// <param name="input">The text of an IP</param>
        /// <returns>An IP address, or null</returns>
        public static IPEndPoint StringToIP(string input)
        {
            if (input.Contains('['))
            {
                if (!input.Contains(']'))
                {
                    return null;
                }
                string[] Data = input.Split(']');
                if (Data.Length != 2)
                {
                    return null;
                }
                if (Data[0].Length == 0)
                {
                    return null;
                }
                if (Data[1].Length == 0)
                {
                    return new IPEndPoint(IPAddress.Parse(Data[0].Substring(1)), 26805);
                }
                else if (Data[1].Length == 1)
                {
                    return null;
                }
                return new IPEndPoint(IPAddress.Parse(Data[0].Substring(1)), Utilities.StringToInt(Data[1].Substring(1)));
            }
            else
            {
                string[] data = input.Split(':');
                string IP = data[0];
                string Port;
                if (data.Length == 2)
                {
                    Port = data[1];
                }
                else if (data.Length == 1)
                {
                    Port = "26805";
                }
                else
                {
                    return new IPEndPoint(IPAddress.Parse(input), 26805);
                }
                string[] IPParts = IP.Split('.');
                if (IPParts.Length != 4)
                {
                    return null;
                }
                byte[] Address = new byte[] { (byte)Utilities.StringToInt(IPParts[0]),
                    (byte)Utilities.StringToInt(IPParts[1]),
                    (byte)Utilities.StringToInt(IPParts[2]),
                    (byte)Utilities.StringToInt(IPParts[3]) };
                return new IPEndPoint(new IPAddress(Address), Utilities.StringToInt(Port));
            }
        }
    }
}

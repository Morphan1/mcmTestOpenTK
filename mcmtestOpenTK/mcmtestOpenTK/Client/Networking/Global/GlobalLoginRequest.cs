using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Shared;
using System.Security.Cryptography;

namespace mcmtestOpenTK.Client.Networking.Global
{
    public class GlobalLoginRequest : GlobalSecureRequest
    {
        /// <summary>
        /// Sends a message to the Global Server, to begin logging in.
        /// </summary>
        /// <param name="Announce">Whether to announce the result in the console</param>
        /// <param name="username">The username to login with</param>
        /// <param name="password">The password to login with</param>
        /// <returns>The GlobalNetwork object created for this time request</returns>
        public static GlobalLoginRequest RequestLogin(bool Announce, string username, string password)
        {
            GlobalLoginRequest glr = new GlobalLoginRequest();
            glr.Username = username;
            glr.Password = password;
            NetworkingObjects.Add(glr);
            glr.Send();
            return glr;
        }

        /// <summary>
        /// Whether the login succeeded.
        /// </summary>
        public bool Success = false;

        /// <summary>
        /// Whether the success/failure state of the login should be announced.
        /// </summary>
        public bool ShouldAnnounce = false;

        /// <summary>
        /// The username to login with.
        /// </summary>
        public string Username;

        /// <summary>
        /// The password to login with.
        /// </summary>
        public string Password;

        public override void Send()
        {
            socket = NetworkUtil.CreateSocket();
            Thread thr = new Thread(new ThreadStart(ConnectMe));
            thr.Start();
        }
        void ConnectMe()
        {
            string result = SecureConnectBase("LOGIN\n" + Username + "\n" + Password);
            if (Error != null)
            {
                return;
            }
            Error = result;
        }

        public override void TickMe()
        {
            lock (Locker)
            {
                if (Error != null)
                {
                    TimeRan = GlobalNetwork.MaxRunTime;
                    KillQuietly = true;
                    UIConsole.WriteLine(TextStyle.Color_Error + "Login failed with message: " + TextStyle.Color_Separate + Error);
                    return;
                }
            }
            if (Success)
            {
                if (ShouldAnnounce)
                {
                    UIConsole.WriteLine(TextStyle.Color_Importantinfo + "Successfully logged in as " +
                        TextStyle.Color_Separate + Username + TextStyle.Color_Importantinfo + "!");
                }
                ready = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using mcmtestOpenTK.Shared;
using System.Security.Cryptography;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.GameHandlers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.Global
{
    public class GlobalSessionRequest : GlobalSecureRequest
    {
        /// <summary>
        /// Sends a message to the Global Server, to check the validity of a session key.
        /// </summary>
        /// <param name="username">The username to check</param>
        /// <param name="session">The session key to check</param>
        /// <returns>The GlobalNetwork object created for this session request</returns>
        public static GlobalSessionRequest RequestSession(Player player, string username, string session)
        {
            GlobalSessionRequest glr = new GlobalSessionRequest();
            glr.Username = username;
            glr.Session = session;
            glr.player = player;
            NetworkingObjects.Add(glr);
            glr.Send();
            return glr;
        }

        /// <summary>
        /// The username to check.
        /// </summary>
        public string Username;

        /// <summary>
        /// The session key to check.
        /// </summary>
        public string Session;

        /// <summary>
        /// The player being checked.
        /// </summary>
        public Player player;

        public override void Send()
        {
            socket = NetworkUtil.CreateSocket();
            Thread thr = new Thread(new ThreadStart(ConnectMe));
            thr.Start();
        }

        void ConnectMe()
        {
            string result = SecureConnectBase("SESSION\n" + Username + "\n" + Session);
            lock (Locker)
            {
                if (Error != null)
                {
                    return;
                }
                Error = result;
            }
        }

        public override void TickMe()
        {
            lock (Locker)
            {
                if (Error != null)
                {
                    TimeRan = GlobalNetwork.MaxRunTime;
                    KillQuietly = true;
                    if (Error.StartsWith("REFUSED:"))
                    {
                        string[] errorsplit = Error.Split(new char[] { ':' }, 2);
                        SysConsole.Output(OutputType.INFO, "Session check was refused with message: " + errorsplit[1]);
                        player.Kick("Invalid session key.");
                    }
                    else if (Error.StartsWith("ACCEPT:"))
                    {
                        player.Identified();
                    }
                    else
                    {
                        SysConsole.Output(OutputType.INFO, "Session check was refused with message: " + Error);
                        player.Kick("Invalid session key.");
                    }
                    ready = true;
                    return;
                }
            }
        }

        public override void Kill()
        {
            player.Kick("Invalid session key.");
            try
            {
                socket.Close();
            }
            catch (Exception)
            {
                // Irrelevant, we already want to scrap this entirely.
            }
        }
    }
}

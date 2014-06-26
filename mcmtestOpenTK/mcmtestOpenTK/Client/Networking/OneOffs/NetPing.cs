using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.Networking.OneOffs
{
    public abstract class NetPing
    {
        /// <summary>
        /// The address of the Global Server.
        /// </summary>
        public static string GlobalAddress = "mcmonkey.org";

        /// <summary>
        /// The port of the Global Server.
        /// </summary>
        public static int GlobalPort = 28005;

        /// <summary>
        /// A full list of all NetPing objects waiting for results.
        /// </summary>
        public static List<NetPing> NetworkingObjects = null;

        /// <summary>
        /// How long a ping request may run for before it's killed.
        /// </summary>
        public const float MaxRunTime = 10f;

        /// <summary>
        /// Initializes the NetPing system.
        /// </summary>
        public static void Init()
        {
            NetworkingObjects = new List<NetPing>();
        }

        /// <summary>
        /// Ticks all waiting NetPing objects, checking for updates.
        /// </summary>
        public static void Tick()
        {
            for (int i = 0; i < NetworkingObjects.Count; i++)
            {
                NetPing gn = NetworkingObjects[i];
                gn.TickMe();
                gn.TimeRan += MainGame.DeltaF;
                if (gn.ready)
                {
                    NetworkingObjects.RemoveAt(i);
                    i--;
                    continue;
                }
                if (gn.TimeRan > MaxRunTime)
                {
                    if (!gn.KillQuietly)
                    {
                        UIConsole.WriteLine(TextStyle.Color_Error + "Ping connection failed (timeout)!");
                    }
                    gn.Kill();
                    NetworkingObjects.RemoveAt(i);
                    i--;
                    continue;
                }
            }
        }

        /// <summary>
        /// Whether the ping operation was completed and has a valid result.
        /// </summary>
        public bool ready = false;

        /// <summary>
        /// How long the ping operation took to run, in seconds.
        /// </summary>
        public float TimeRan = 0f;

        /// <summary>
        /// If true, nothing will be announced when the object is timed-out.
        /// </summary>
        public bool KillQuietly = false;

        /// <summary>
        /// Tick the ping operation appropriately.
        /// </summary>
        public abstract void TickMe();

        /// <summary>
        /// Initializes the ping operation and sends it off to the target server.
        /// </summary>
        public abstract void Send();

        /// <summary>
        /// Properly kill off and fail the request.
        /// </summary>
        public abstract void Kill();
    }
}

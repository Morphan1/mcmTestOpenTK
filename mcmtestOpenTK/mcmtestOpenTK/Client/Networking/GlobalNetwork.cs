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

namespace mcmtestOpenTK.Client.Networking
{
    public abstract class GlobalNetwork
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
        /// A full list of all GlobalNetwork objects waiting for results.
        /// </summary>
        public static List<GlobalNetwork> NetworkingObjects = null;

        /// <summary>
        /// How long a Global Network request may run for before it's killed.
        /// </summary>
        public const float MaxRunTime = 10f;

        /// <summary>
        /// Initializes the Global Network system.
        /// </summary>
        public static void Init()
        {
            NetworkingObjects = new List<GlobalNetwork>();
        }

        /// <summary>
        /// Ticks all waiting GlobalNetwork objects, checking for updates.
        /// </summary>
        public static void Tick()
        {
            for (int i = 0; i < NetworkingObjects.Count; i++)
            {
                GlobalNetwork gn = NetworkingObjects[i];
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
                    UIConsole.WriteLine(TextStyle.Color_Error + "Global network connection failed (timeout)!");
                    gn.Kill();
                    NetworkingObjects.RemoveAt(i);
                    i--;
                    continue;
                }
            }
        }

        /// <summary>
        /// Whether the Global Network operation was completed and has a valid result.
        /// </summary>
        public bool ready = false;

        /// <summary>
        /// How long the Global Network operation took to run, in seconds.
        /// </summary>
        public float TimeRan = 0f;

        /// <summary>
        /// Tick the Global Network operation appropriately.
        /// </summary>
        public abstract void TickMe();

        /// <summary>
        /// Initializes the Global Network operation and sends it off to the Global Server.
        /// </summary>
        public abstract void Send();

        /// <summary>
        /// Properly kill off and fail the request.
        /// </summary>
        public abstract void Kill();
    }
}

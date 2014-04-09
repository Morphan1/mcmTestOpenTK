using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        /// <summary>
        /// Spawns a new entity into the world.
        /// </summary>
        /// <param name="e">The entity to spawn.</param>
        public static void SpawnEntity(Entity e)
        {
            if (!entities.Contains(e))
            {
                entities.Add(e);
            }
        }

        static long cID = 0;
        /// <summary>
        /// Selects a new semi-unique Entity ID.
        /// </summary>
        /// <returns>A new semi-unique Entity ID.</returns>
        public static long NewEntityID()
        {
            if (cID == long.MaxValue)
            {
                cID = long.MinValue;
                ErrorHandler.HandleError("Current Entity ID maxed out and looped!");
            }
            cID++;
            return cID;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.GlobalHandler
{
    public partial class MainGame
    {
        /// <summary>
        /// A list of all entities in the world.
        /// </summary>
        public static List<Entity> Entities;

        /// <summary>
        /// A list of all Ticking entities in the world.
        /// </summary>
        public static List<Entity> Tickers;

        /// <summary>
        /// A list of all solid entities in the world.
        /// </summary>
        public static List<Entity> Solids;

        static Skybox skybox = null;

        static void LoadWorld()
        {
            Entities = new List<Entity>();
            Tickers = new List<Entity>();
            Solids = new List<Entity>();
            skybox = new Skybox();
            skybox.Init();
        }

        /// <summary>
        /// Spawns a new entity into the world.
        /// </summary>
        /// <param name="e">The entity to spawn</param>
        public static void SpawnEntity(Entity e)
        {
            if (e.IsCorrupt)
            {
                return;
            }
            if (!Entities.Contains(e))
            {
                Entities.Add(e);
                e.IsValid = true;
                if (e.TickMe)
                {
                    Tickers.Add(e);
                }
                if (e.Solid)
                {
                    Solids.Add(e);
                }
            }
        }

        /// <summary>
        /// Spawns a new entity of the given type.
        /// </summary>
        /// <param name="e">The type of entity to spawn</param>
        /// <param name="ID">The entity ID to use</param>
        /// <param name="Position">Where to spawn it</param>
        /// <param name="Data">Optional - network binary data describing the entity</param>
        public static void SpawnEntity(EntityType e, ulong ID, Location Position, byte[] Data = null)
        {
            Entity ent = Entity.FromType(e);
            if (ent == null)
            {
                UIConsole.WriteLine("Tried to spawn unknown entity type: " + (int)e);
                return;
            }
            ent.UniqueID = ID;
            ent.Position = Position;
            ent.ReadBytes(Data);
            SpawnEntity(ent);
        }

        /// <summary>
        /// Gets the entity that has the given ID.
        /// </summary>
        /// <param name="ID">The ID to find an entity for</param>
        /// <returns>An entity, or null</returns>
        public static Entity GetEntity(ulong ID)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                if (Entities[i].UniqueID == ID)
                {
                    return Entities[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Removes an entity from the world.
        /// </summary>
        /// <param name="e">The entity to remove</param>
        public static void Destroy(Entity e)
        {
            Entities.Remove(e);
            e.IsValid = false;
            if (e.TickMe)
            {
                Tickers.Remove(e);
            }
        }

        /// <summary>
        /// Clears the entire world.
        /// </summary>
        public static void DestroyWorld()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].IsValid = false;
            }
            Entities = new List<Entity>();
            Tickers = new List<Entity>();
        }

        static void TickWorld()
        {
            // Update all entities
            for (int i = 0; i < Tickers.Count; i++)
            {
                Tickers[i].Tick();
            }
        }

        static void DrawWorld()
        {
            skybox.Draw();
            Shader.Generic.Bind();
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Draw();
            }
        }
    }
}

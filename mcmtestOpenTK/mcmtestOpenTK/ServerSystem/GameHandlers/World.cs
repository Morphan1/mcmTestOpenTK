using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GameHandlers.GameHelpers;

namespace mcmtestOpenTK.ServerSystem.GameHandlers
{
    public class World
    {
        /// <summary>
        /// The name of the world.
        /// </summary>
        public string Name;

        /// <summary>
        /// A list of all players inside the world.
        /// </summary>
        public List<Player> Players;

        /// <summary>
        /// A list of all entities inside the world.
        /// </summary>
        public List<Entity> Entities;

        /// <summary>
        /// A list of all entities inside the world that need ticking.
        /// </summary>
        public List<Entity> Tickers;

        public World(string _name)
        {
            Name = _name;
            Players = new List<Player>();
            Entities = new List<Entity>();
            Tickers = new List<Entity>();
        }

        /// <summary>
        /// Loads up the world properly.
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// Ticks all online entities, and the world itself.
        /// </summary>
        public void Tick()
        {
            for (int i = 0; i < Tickers.Count; i++)
            {
                Entity e = Tickers[i];
                e.Tick();
                if (!e.Valid)
                {
                    i--;
                }
            }
        }

        /// <summary>
        /// Spawns an entity into the world.
        /// </summary>
        /// <param name="entity">The entity to spawn</param>
        public void Spawn(Entity entity)
        {
            if (entity.Valid)
            {
                return;
            }
            entity.world = this;
            entity.UniqueID = GetUniqueID();
            entity.Valid = true;
            Entities.Add(entity);
            if (entity.TickMe)
            {
                Tickers.Add(entity);
            }
            if (entity is Player)
            {
                Players.Add((Player)entity);
            }
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].Send(new SpawnPacketOut(entity));
            }
        }

        /// <summary>
        /// Removes an entity from the world.
        /// </summary>
        /// <param name="ent">The entity to remove</param>
        public void Destroy(Entity ent)
        {
            ent.Kill();
            Entities.Remove(ent);
            if (ent is Player)
            {
                Players.Remove((Player)ent);
            }
            if (ent.TickMe)
            {
                Tickers.Remove(ent);
            }
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].Send(new DespawnPacketOut(ent.UniqueID));
            }
        }

        /// <summary>
        /// Gets the entity that has the specified ID.
        /// </summary>
        /// <param name="ID">The ID of the entity to get.</param>
        public Entity GetEntity(ulong ID)
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

        ulong cID = 0;

        bool MustVerifyIDs = false;

        /// <summary>
        /// Determines whether an entity by the given ID is already in the world.
        /// </summary>
        /// <param name="ID">The ID to check</param>
        /// <returns>Whether such an entity exists</returns>
        public bool EntityExists(ulong ID)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                if (Entities[i].UniqueID == ID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets a new world-unique ID.
        /// </summary>
        /// <returns>The gotten ID</returns>
        public ulong GetUniqueID()
        {
            if (cID == ulong.MaxValue)
            {
                SysConsole.Output(OutputType.WARNING, "[" + Name + "] Used up all unique IDs! Restart the server to regain run-speed!");
                cID = 0;
                MustVerifyIDs = true;
            }
            if (MustVerifyIDs)
            {
                bool flip = false;
                while (EntityExists(cID))
                {
                    if (cID == ulong.MaxValue)
                    {
                        cID = 0;
                        if (flip)
                        {
                            SysConsole.Output(OutputType.WARNING, "[" + Name + "] Impossibly high number of entities... destructing!");
                            DestroyWorld();
                        }
                        flip = true;
                    }
                    cID++;
                }
                return cID;
            }
            else
            {
                return cID++;
            }
        }

        public bool HasMap = false;

        /// <summary>
        /// Destroys the world, releasing all memory.
        /// </summary>
        public void DestroyWorld(bool quiet = false)
        {
            if (!quiet)
            {
                SysConsole.Output(OutputType.INFO, "[" + Name + "] Destructing...");
            }
            while (Entities.Count > 0)
            {
                Destroy(Entities[0]);
            }
            Entities.Clear();
            Tickers.Clear();
            Players.Clear();
            if (!quiet)
            {
                SysConsole.Output(OutputType.INFO, "[" + Name + "] Gone!");
            }
        }
    }
}

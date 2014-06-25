using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;

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

        /// <summary>
        /// A list of all entities that serve as spawn points.
        /// </summary>
        public List<SpawnPoint> SpawnPoints;

        public World(string _name)
        {
            Name = _name;
            Players = new List<Player>();
            Entities = new List<Entity>();
            Tickers = new List<Entity>();
            SpawnPoints = new List<SpawnPoint>();
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
                bool WasSolid = e.Solid;
                e.Solid = false;
                e.Tick();
                e.Solid = WasSolid;
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
            int playerindex = -1;
            if (entity is Player)
            {
                Players.Add((Player)entity);
                playerindex = Players.Count - 1;
            }
            else if (entity is SpawnPoint)
            {
                SpawnPoints.Add((SpawnPoint)entity);
            }
            if (entity.NetTransmit)
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    if (i != playerindex)
                    {
                        Players[i].Send(new SpawnPacketOut(entity));
                    }
                }
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
            else if (ent is SpawnPoint)
            {
                SpawnPoints.Remove((SpawnPoint)ent);
            }
            if (ent.TickMe)
            {
                Tickers.Remove(ent);
            }
            if (ent.NetTransmit)
            {
                for (int i = 0; i < Players.Count; i++)
                {
                    Players[i].Send(new DespawnPacketOut(ent.UniqueID));
                }
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

        /// <summary>
        /// Finds a place to spawn a player at.
        /// </summary>
        /// <returns>A valid spawn location</returns>
        public Location FindSpawnPoint()
        {
            // Make sure we have spawn points
            if (SpawnPoints.Count == 0)
            {
                SysConsole.Output(OutputType.ERROR, "World lacks a spawn point! Adding one at (0, 0, 1)");
                Spawn(new SpawnPoint() { Position = new Location(0, 0, 1) });
            }
            // Loop a few times to look for an open spawn
            for (int tries = 0; tries < SpawnPoints.Count * 2; tries++)
            {
                SpawnPoint sp = SpawnPoints[Utilities.random.Next(SpawnPoints.Count)];
                if (!sp.IsBlocked())
                {
                    return sp.Position;
                }
            }
            // Oh dear! Most of them are blocked! Pick one at random and hope something horrible doesn't happen.
            return SpawnPoints[Utilities.random.Next(SpawnPoints.Count)].Position;
        }

        /// <summary>
        /// Whether a map is loaded onto the world currently.
        /// </summary>
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
            int ignore = 0;
            while (Entities.Count > ignore)
            {
                if (Entities[0] is Player)
                {
                    ignore++;
                    continue;
                }
                Destroy(Entities[0]);
            }
            if (!quiet)
            {
                SysConsole.Output(OutputType.INFO, "[" + Name + "] Gone!");
            }
        }
    }
}

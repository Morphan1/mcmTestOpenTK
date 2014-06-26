using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.NetworkHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn;

namespace mcmtestOpenTK.ServerSystem.GameHandlers.Entities
{
    public class Player: MovingEntity
    {
        /// <summary>
        /// The default collision mins for a player.
        /// </summary>
        public static Location DefaultMins = new Location(-1.5f, -1.5f, 0);

        /// <summary>
        /// The default collision maxes for a player.
        /// </summary>
        public static Location DefaultMaxes = new Location(1.5f, 1.5f, 8f);

        public Player(): base(EntityType.PLAYER, true)
        {
            ToSend = new List<byte[]>();
            Solid = true;
            Mins = DefaultMins;
            Maxs = DefaultMaxes;
            PlayerGravity = 100;
            CheckCollision = true;
            MoveType = MovementType.SlideBox;
        }

        /// <summary>
        /// The network Connection object for this player.
        /// </summary>
        public NewConnection Network;

        /// <summary>
        /// The current Ping ID, for networking.
        /// </summary>
        public byte PingID;

        /// <summary>
        /// Indicates that the player hasn't been kicked or dropped.
        /// </summary>
        public bool IsAlive = true;

        /// <summary>
        /// The name of this player.
        /// </summary>
        public string Username = "";

        /// <summary>
        /// The session this player used to log in.
        /// </summary>
        public string Session = "";

        /// <summary>
        /// Whether the user has successfully logged in.
        /// </summary>
        public bool IsIdentified = false;

        /// <summary>
        /// A list of all packets waiting to be sent.
        /// </summary>
        public List<byte[]> ToSend;

        /// <summary>
        /// Whether this player is in noclip mode.
        /// </summary>
        public bool Noclip = false;

        /// <summary>
        /// What the player's gravity is (Do not directly set Gravity!)
        /// </summary>
        public float PlayerGravity = 100;

        /// <summary>
        /// Call when the user has fully identified, to let them into the server.
        /// </summary>
        public void Identified()
        {
            if (IsIdentified)
            {
                return;
            }
            SysConsole.Output(OutputType.INFO, "Client '" + Username + "' is now identified!");
            IsIdentified = true;
            Send(new PingPacketOut(this));
            Spawn(Server.MainWorld);
        }

        void Spawn(World world)
        {
            Position = world.FindSpawnPoint();
            Teleport(Position);
            world.Spawn(this);
            NetStringManager.AnnounceAll(this);
            for (int i = 0; i < world.Entities.Count; i++)
            {
                if (world.Entities[i].NetTransmit && world.Entities[i] != this)
                {
                    Send(new SpawnPacketOut(world.Entities[i]));
                }
            }
        }

        /// <summary>
        /// Immediately kicks the player for the specified reason.
        /// </summary>
        /// <param name="reason">Why the player was kicked.</param>
        public void Kick(string reason)
        {
            if (IsAlive)
            {
                Send(new DisconnectPacketOut(reason));
                SysConsole.Output(OutputType.INFO, "Player " + Username + "/" + Network.IP + " was kicked: " + reason);
                IsAlive = false;
                Network.Disconnect();
            }
        }

        /// <summary>
        /// Sends a data-packet to the player.
        /// </summary>
        /// <param name="Packet">The packet to send</param>
        public void Send(AbstractPacketOut Packet)
        {
            if (!IsAlive)
            {
                return;
            }
            PlayerHandler.Send(this, Packet);
        }

        /// <summary>
        /// Shows a simple textual message in the player's console.
        /// </summary>
        /// <param name="message">The message to show</param>
        public void SendMessage(string message)
        {
            Send(new MessagePacketOut(message));
        }

        /// <summary>
        /// Teleports the player to the specified location.
        /// </summary>
        /// <param name="loc">The location to teleport to</param>
        public void Teleport(Location loc)
        {
            Position = loc;
            LastMoveLoc = loc;
            Velocity = Location.Zero;
            LastVelocity = Location.Zero;
            Send(new TeleportPacketOut(loc));
        }

        /// <summary>
        /// Prepares the player.
        /// </summary>
        public void Init()
        {
            LastPacket = new MovementPacketIn();
            LastPacket.Time = Server.GlobalTickTime;
            LastMovement = Server.GlobalTickTime;
            Send(new HelloPacketOut());
            Send(new TimePacketOut());
        }

        public bool Forward = false;
        public bool Back = false;
        public bool Left = false;
        public bool Right = false;
        public bool Up = false;
        public bool Down = false;

        public override void Tick()
        {
            Tick(Server.DeltaF, false);
        }

        public override void Tick(float MyDelta, bool isCustom)
        {
            if (!IsAlive || !Network.IsAlive)
            {
                IsAlive = false;
                Network.Disconnect();
                world.Destroy(this);
                return;
            }
            if (!isCustom)
            {
                int count = PacketsToApply.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        PacketsToApply[i].Execute(this);
                    }
                    PacketsToApply.RemoveRange(0, count);
                }
            }
            Location movement = Location.Zero;
            if (Left)
            {
                movement.Y = -1;
            }
            if (Right)
            {
                movement.Y = 1;
            }
            if (Back)
            {
                if (movement.Y != 0)
                {
                    movement.Y *= 0.5f;
                    movement.X = 0.5f;
                }
                else
                {
                    movement.X = 1;
                }
            }
            if (Forward)
            {
                if (movement.Y != 0)
                {
                    movement.Y *= 0.5f;
                    movement.X = -0.5f;
                }
                else
                {
                    movement.X = -1;
                }
            }
            if (Noclip)
            {
                Gravity = 0;
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180, Direction.Y * Utilities.PI180);
                }
                if (Up)
                {
                    movement.Z = 1;
                }
                if (Down)
                {
                    movement.Z -= 1;
                }
                Velocity = movement * 30;
            }
            else
            {
                Gravity = PlayerGravity;
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180);
                }
                if (Down)
                {
                    Velocity.Z = 0;
                    Position.Z = 20;
                }
                if (Up)
                {
                    if (Velocity.Z < 0.0001f && Velocity.Z > -0.0001f
                        && Collision.Box(Position, new Location(-1.5f, -1.5f, -0.5f), new Location(1.5f, 1.5f, 2)))
                    {
                        Velocity.Z = 50;
                    }
                }
                Velocity = new Location(movement.X * 30, movement.Y * 30, Velocity.Z);
            }
            base.Tick(MyDelta, isCustom);
            if (!isCustom)
            {
                LastTick = Server.GlobalTickTime;
            }
        }

        public double LastTick;
        public double LastMovement;
        Location LastMoveLoc;
        Location LastVelocity;
        MovementPacketIn LastPacket;

        public List<MovementPacketIn> PacketsToApply = new List<MovementPacketIn>();

        public void ApplyNewMovement(double MoveTime, MovementPacketIn pack)
        {
            bool WasSolid = Solid;
            Solid = false;
            // Apply last known position / movement.
            Position = LastMoveLoc;
            Velocity = LastVelocity;
            MovementPacketIn.ApplyPosition(this, LastPacket.movement, LastPacket.yaw, LastPacket.pitch);
            // Tick from last known movement to new position.
            float targetdelta = (float)(MoveTime - LastMovement);
            while (targetdelta > 1f / 60f)
            {
                Tick(1f / 60f, true);
                targetdelta -= 1f / 60f;
            }
            Tick(targetdelta, true);
            // Tell the player where they were at when the packet arrived.
            LastMoveLoc = Position;
            LastVelocity = Velocity;
            LastMovement = MoveTime;
            Send(new YourPositionPacketOut(MoveTime, Position, Velocity));
            LastPacket = pack;
            // Apply the new movement packet.
            MovementPacketIn.ApplyPosition(this, pack.movement, pack.yaw, pack.pitch);
            // Tick back up to now.
            targetdelta = (float)(LastTick - MoveTime);
            while (targetdelta > 1f / 60f)
            {
                Tick(1f / 60f, true);
                targetdelta -= 1f / 60f;
            }
            Tick(targetdelta, true);
            Solid = WasSolid;
        }

        public override void Kill()
        {
        }

        public override byte[] GetData()
        {
            // TODO: Name, etc.
            return new byte[0];
        }

        public override bool HandleVariable(string varname, string vardata)
        {
            throw new InvalidOperationException("Tried to apply variable to Player entity!");
        }

        public override List<Variable> GetSaveVars()
        {
            throw new InvalidOperationException("Tried to get variables of a Player entity!");
        }

        public override void Collide()
        {
        }
    }
}

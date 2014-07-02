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
    public class Player: Entity
    {
        /// <summary>
        /// The default collision mins for a player.
        /// </summary>
        public static Location DefaultMins = new Location(-1.5f, -1.5f, 0);

        /// <summary>
        /// The default collision maxes for a player.
        /// </summary>
        public static Location DefaultMaxes = new Location(1.5f, 1.5f, 8f);

        public Player(): base(true, true, EntityType.PLAYER)
        {
            ToSend = new List<byte[]>();
            Solid = true;
            Mins = DefaultMins;
            Maxs = DefaultMaxes;
            Gravity = 100;
        }

        /// <summary>
        /// The network Connection object for this player.
        /// </summary>
        public NewConnection Network;

        /// <summary>
        /// The movement velocity of the player.
        /// </summary>
        public Location Velocity;

        /// <summary>
        /// The direction the player is facing.
        /// </summary>
        public Location Direction;

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
        /// What the player's gravity strength is.
        /// </summary>
        public float Gravity = 100;

        /// <summary>
        /// All keys saved on the user.
        /// </summary>
        public Dictionary<string, string> AccountKeys;

        public List<string> permissions;

        /// <summary>
        /// Returns whether the player has the permission node, or a supernode of it.
        /// </summary>
        /// <param name="permission">The permission to check for</param>
        /// <returns>Whether the user has the permission</returns>
        public bool HasPermission(string permission)
        {
            // Only identified users have any permissions
            if (!IsIdentified)
            {
                return false;
            }
            permission = permission.ToLower();
            if (permissions.Contains(permission))
            {
                return true;
            }
            string[] blocks = permission.Split('.');
            string construct = "";
            for (int i = 0; i < blocks.Length; i++)
            {
                if (permissions.Contains(construct + "*"))
                {
                    return true;
                }
                construct += blocks[i] + ".";
            }
            return false;
        }

        /// <summary>
        /// Call when the user has fully identified, to let them into the server.
        /// </summary>
        public void Identified()
        {
            // Don't double-identify
            if (IsIdentified)
            {
                return;
            }
            // announce
            SysConsole.Output(OutputType.INFO, "Client '" + Username + "' is now identified!");
            // kick duplicate players
            string ulow = Username.ToLower();
            for (int i = 0; i < Server.MainWorld.Players.Count; i++)
            {
                if (Server.MainWorld.Players[i].Username.ToLower() == ulow)
                {
                    Server.MainWorld.Players[i].Kick("Logged in from another location");
                }
            }
            IsIdentified = true;
            // Load user file
            AccountKeys = new Dictionary<string, string>();
            if (FileHandler.Exists("users/" + ulow + ".cfg"))
            {
                string[] data = FileHandler.ReadText("users/" + ulow + ".cfg").Split('\n');
                foreach (string str in data)
                {
                    if (str.Length > 0 && str.Contains(':'))
                    {
                        string[] split = str.Split(':');
                        AccountKeys.Add(Unescape(split[0]), Unescape(split[1]));
                    }
                }
            }
            else
            {
                // USER JOINED FIRST TIME, DO SPECIAL WELCOME STUFF HERE
                AccountKeys.Add("first_name", Username);
                AccountKeys.Add("first_ip", Network.IP);
                AccountKeys.Add("permissions", "basic.new_user");
            }
            // Establish permissions
            permissions = new List<string>(AccountKeys["permissions"].Split(','));
            // start the ping loop
            Send(new PingPacketOut(this));
            // Spawn into world
            Spawn(Server.MainWorld);
        }

        public override void Kill()
        {
            SysConsole.Output(OutputType.INFO, "Saving client '" + Username + "'");
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> x in AccountKeys)
            {
                sb.Append(Escape(x.Key)).Append(":").Append(Escape(x.Value)).Append("\n");
            }
            FileHandler.WriteText("users/" + Username.ToLower() + ".cfg", sb.ToString());
        }

        public string Unescape(string input)
        {
            return input.Replace("&co;", ":").Replace("&nl;", "\n").Replace("&amp;", "&");
        }

        public string Escape(string input)
        {
            return input.Replace("&", "&amp;").Replace("\n", "&nl;").Replace(":", "&co;");
        }

        void Spawn(World world)
        {
            Location dir;
            Position = world.FindSpawnPoint(out dir);
            Teleport(Position, dir);
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
        /// <param name="direction">The direction to face</param>
        public void Teleport(Location loc, Location direction)
        {
            Position = loc;
            LastMoveLoc = loc;
            Velocity = Location.Zero;
            LastVelocity = Location.Zero;
            Send(new TeleportPacketOut(loc, direction));
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
        public bool Slow = false;
        public bool Jumped = false;

        public override void Tick()
        {
            Tick(Server.Delta, false);
        }

        public const double MoveSpeed = 35;
        public const double JumpPower = 50;
        public const double AirSpeedMult = 0.05f;

        public void Tick(double MyDelta, bool isCustom)
        {
            if (MyDelta == 0)
            {
                return;
            }
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
            if (Down)
            {
                Maxs = new Location(1.5f, 1.5f, 5);
            }
            else
            {
                if (!Collision.Box(Position, new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8)))
                {
                    Maxs = new Location(1.5f, 1.5f, 8);
                }
                else
                {
                    Down = true;
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
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180);
                }
                bool on_ground = Velocity.Z < 0.01f && Collision.Box(Position, new Location(-1.5f, -1.5f, -0.01f), new Location(1.5f, 1.5f, 2));
                if (Up && on_ground && !Jumped)
                {
                    Velocity.Z = JumpPower * (Down ? 0.5 : 1);
                    Jumped = true;
                }
                else if (!Up && Jumped)
                {
                    Jumped = false;
                }
                Velocity.X += ((movement.X * MoveSpeed * (Slow || Down ? 0.5 : 1)) - Velocity.X) * MyDelta * 8 * (on_ground ? 1 : AirSpeedMult);
                Velocity.Y += ((movement.Y * MoveSpeed * (Slow || Down ? 0.5 : 1)) - Velocity.Y) * MyDelta * 8 * (on_ground ? 1 : AirSpeedMult);
                Velocity.Z -= Gravity * MyDelta;
            }
            Location ploc = Position;
            Location target = Position + Velocity * MyDelta;
            Position = Collision.SlideBox(Position, target, new Location(-1.5f, -1.5f, 0), Maxs);
            Velocity = (Position - ploc) / MyDelta;
            if (!isCustom)
            {
                LastTick = Server.GlobalTickTime;
                // TODO: Better player transmission - transmit move byte with time, in addition to position/vel/dir?
                /*PositionPacketOut pack = new PositionPacketOut(this, Position, Velocity, Direction);
                for (int i = 0; i < world.Players.Count; i++)
                {
                    if (world.Players[i] != this)
                    {
                        world.Players[i].Send(pack);
                    }
                }*/
            }
        }

        public double LastTick;
        public double LastMovement;
        Location LastMoveLoc;
        Location LastVelocity;
        MovementPacketIn LastPacket;
        public bool LastJumped = false;

        public List<MovementPacketIn> PacketsToApply = new List<MovementPacketIn>();

        public void ApplyNewMovement(double MoveTime, MovementPacketIn pack)
        {
            bool WasSolid = Solid;
            Solid = false;
            // Apply last known position / movement.
            Position = LastMoveLoc;
            Velocity = LastVelocity;
            Jumped = LastJumped;
            MovementPacketIn.ApplyPosition(this, LastPacket.movement, LastPacket.yaw, LastPacket.pitch);
            // Tick from last known movement to new position.
            double targetdelta = MoveTime - LastMovement;
            while (targetdelta > 1d / 60d)
            {
                Tick(1d / 60d, true);
                targetdelta -= 1d / 60d;
            }
            Tick(targetdelta, true);
            // Tell the player where they were at when the packet arrived.
            LastMoveLoc = Position;
            LastVelocity = Velocity;
            LastMovement = MoveTime;
            LastJumped = Jumped;
            Send(new YourPositionPacketOut(MoveTime, Position, Velocity, Jumped));
            LastPacket = pack;
            // Apply the new movement packet.
            MovementPacketIn.ApplyPosition(this, pack.movement, pack.yaw, pack.pitch);
            // Tell all other players of this player's movement
            PlayerPositionPacketOut pout = new PlayerPositionPacketOut(this, Position, Velocity, Direction, pack.movement);
            for (int i = 0; i < world.Players.Count; i++)
            {
                if (world.Players[i] != this)
                {
                    world.Players[i].Send(pout);
                }
            }
            // Tick back up to now.
            targetdelta = (float)(LastTick - MoveTime);
            while (targetdelta > 1d / 60d)
            {
                Tick(1d / 60d, true);
                targetdelta -= 1d / 60d;
            }
            Tick(targetdelta, true);
            Solid = WasSolid;
        }

        public void UpdateStatus()
        {
            byte[] data = new byte[] { Noclip ? (byte)1 : (byte)0 };
            NewdataPacketOut pack = new NewdataPacketOut(this, data);
            for (int i = 0; i < world.Players.Count; i++)
            {
                if (world.Players[i] != this)
                {
                    world.Players[i].Send(pack);
                }
            }
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
    }
}

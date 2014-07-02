using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Input;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;

namespace mcmtestOpenTK.Client.GameplayHandlers
{
    public class Player
    {
        /// <summary>
        /// The default player - there's only ever one!
        /// </summary>
        public static Player player;

        /// <summary>
        /// The precise X/Y/Z location of the entity.
        /// </summary>
        public Location Position = Location.Zero;

        /// <summary>
        /// How much health the player currently has.
        /// </summary>
        public float Health = 0;

        /// <summary>
        /// The precise X/Y/Z worldly movement speed.
        /// </summary>
        public Location Velocity = Location.Zero;

        /// <summary>
        /// The precise Yaw/Pitch/Roll direction of the entity.
        /// </summary>
        public Location Direction = Location.Zero;

        public bool forward = false;
        public bool back = false;
        public bool left = false;
        public bool right = false;
        public bool up = false;
        public bool down = false;
        public bool slow = false;
        public bool jumped = false;

        public const double MoveSpeed = 35;
        public const double BaseGravity = 100;
        public const double JumpPower = 50;
        public const double AirSpeedMult = 0.05f;

        public Location Maxs;

        /// <summary>
        /// Called to tick the default player.
        /// </summary>
        public void Update(double MyDelta, bool IsCustom)
        {
            if (MyDelta == 0)
            {
                return;
            }
            // Don't move if not even spawned.
            if (!MainGame.Spawned)
            {
                if (MouseHandler.MouseCaptured)
                {
                    MouseHandler.ReleaseMouse();
                }
                return;
            }
            // Mouse based rotation
            if (!IsCustom)
            {
                Direction.X += MouseHandler.MouseDelta.X;
                Direction.Y += MouseHandler.MouseDelta.Y;
            }
            while (Direction.X < 0)
            {
                Direction.X += 360;
            }
            while (Direction.X > 360)
            {
                Direction.X -= 360;
            }
            if (Direction.Y > 80)
            {
                Direction.Y = 80;
            }
            if (Direction.Y < -80)
            {
                Direction.Y = -80;
            }
            if (!IsCustom)
            {
                MainGame.Forward = Utilities.ForwardVector(Direction.X * Utilities.PI180, Direction.Y * Utilities.PI180);
            }
            // Keyboard based movement.
            Location movement = Location.Zero;
            if (!IsCustom)
            {
                left = KeyHandler.KeyBindIsDown(KeyBind.LEFT);
                right = KeyHandler.KeyBindIsDown(KeyBind.RIGHT);
                forward = KeyHandler.KeyBindIsDown(KeyBind.FORWARD);
                back = KeyHandler.KeyBindIsDown(KeyBind.BACK);
                up = KeyHandler.KeyBindIsDown(KeyBind.UP);
                down = KeyHandler.KeyBindIsDown(KeyBind.DOWN);
                slow = KeyHandler.KeyBindIsDown(KeyBind.SLOW);
            }
            if (left)
            {
                movement.Y = -1;
            }
            if (right)
            {
                movement.Y = 1;
            }
            if (back)
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
            if (forward)
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
            if (down)
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
                    down = true;
                }
            }
            bool justJumped = false;
            if (ClientCVar.g_noclip.ValueB)
            {
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180, Direction.Y * Utilities.PI180);
                }
                if (up)
                {
                    movement.Z = 1;
                }
                if (down)
                {
                    movement.Z -= 1;
                }
                Velocity = movement * MoveSpeed;
            }
            else
            {
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180);
                }
                bool on_ground = Velocity.Z < 0.0001f && Collision.Box(Position, new Location(-1.5f, -1.5f, -0.01f), new Location(1.5f, 1.5f, 2));
                if (up && on_ground && !jumped)
                {
                    Velocity.Z = JumpPower * (down ? 0.5 : 1);
                    //jumped = true;
                    //justJumped = true;
                }
                /*else if (!up && jumped)
                {
                    jumped = false;
                }*/
                if (!IsCustom)
                {
                    onground = on_ground;
                }
                Velocity.X += ((movement.X * MoveSpeed * (slow || down ? 0.5: 1)) - Velocity.X) * MyDelta * 8 * (on_ground ? 1 : AirSpeedMult);
                Velocity.Y += ((movement.Y * MoveSpeed * (slow || down ? 0.5 : 1)) - Velocity.Y) * MyDelta * 8 * (on_ground ? 1 : AirSpeedMult);
                Velocity.Z -= BaseGravity * MyDelta;
            }
            Location target = Position + Velocity * MyDelta;
            Location ploc = Position;
            Position = Collision.SlideBox(Position, target, new Location(-1.5f, -1.5f, 0), Maxs);
            Velocity = (Position - ploc) / MyDelta;
            if (!IsCustom)
            {
                //MainGame.SpawnEntity(new Bullet() { Position = Position, LifeTicks = 600, texture = Texture.White, start = ploc });
                ushort move = MovementPacketOut.GetControlShort(forward, back, left, right, up && (!jumped || justJumped), down, slow);
                reps++;
                if (move != lastMove || Direction != lastdir || Velocity != lastvel || reps > 0)
                {
                    lastMove = move;
                    lastdir = Location.Zero;
                    lastvel = Velocity;
                    reps = 0;
                    if (NetworkBase.IsActive)
                    {
                        Points.Add(CPoint());
                        if (Points.Count > 60)
                        {
                            Points.RemoveAt(0);
                        }
                        NetworkBase.Send(new MovementPacketOut(MainGame.GlobalTickTime, move, (float)Direction.X, (float)Direction.Y));
                    }
                }
            }
        }

        public bool onground = false;

        public BroadcastPoint CPoint()
        {
            return new BroadcastPoint(MainGame.GlobalTickTime, Position, Velocity, Direction, forward, back, left, right, up, down, slow, jumped);
        }

        List<BroadcastPoint> Points = new List<BroadcastPoint>(65);

        public void ApplyMovement(Location pos, Location vel, double Time)
        {
            // Loop through all points in reverse
            for (int i = Points.Count - 1; i >= 0; i--)
            {
                // At the first point found that is before the target
                if (Points[i].Time < Time)
                {
                    // Record our current point (probably not needed, just in case)
                    BroadcastPoint cpoint = CPoint();
                    // Apply the last point fully
                    Points[i].Apply(this);
                    Position = Points[i].Position;
                    Velocity = Points[i].Velocity;
                    double ctime = Points[i].Time;
                    double Target = Time - ctime;
                    while (Target > 1d / 60d)
                    {
                        Update(1d / 60d, true);
                        Target -= 1d / 60d;
                    }
                    Update(Target, true);
                    ctime = Time;
                    // Apply changes
                    Position += (pos - Position);
                    Velocity += (vel - Velocity);
                    // Loop through all future points
                    for (int x = i + 1; x < Points.Count; x++)
                    {
                        // Apply the movement from the last point to this one
                        Target = Points[x].Time - ctime;
                        while (Target > 1f / 60f)
                        {
                            Update(1d / 60d, true);
                            Target -= 1d / 60d;
                        }
                        Update(Target, true);
                        // Apply this point for the next calculation
                        Points[x].Apply(this);
                        ctime = Points[x].Time;
                    }
                    // Restore our keys to what they should be (probably not needed, just in case)
                    cpoint.Apply(this);
                    break;
                }
            }
        }

        int reps = 0;
        ushort lastMove = 0;
        Location lastdir = Location.Zero;
        Location lastvel = Location.Zero;
    }

    public class BroadcastPoint
    {
        public double Time;
        public Location Position;
        public Location Velocity;
        public Location Direction;
        public bool left;
        public bool right;
        public bool forward;
        public bool back;
        public bool up;
        public bool down;
        public bool slow;
        public bool jumped;
        public BroadcastPoint(double _time, Location _position, Location _velocity, Location _direction,
            bool _forward, bool _back, bool _left, bool _right, bool _up, bool _down, bool _slow, bool _jumped)
        {
            Direction = _direction;
            Time = _time;
            Position = _position;
            Velocity = _velocity;
            forward = _forward;
            back = _back;
            left = _left;
            right = _right;
            up = _up;
            down = _down;
            slow = _slow;
            jumped = _jumped;
        }
        public void Apply(Player player)
        {
            player.Direction = Direction;
            player.forward = forward;
            player.back = back;
            player.left = left;
            player.right = right;
            player.up = up;
            player.down = down;
            player.jumped = jumped;
        }
    }
}

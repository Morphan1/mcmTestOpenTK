﻿using System;
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

        /// <summary>
        /// Called to tick the default player.
        /// </summary>
        public void Update(float MyDelta, bool IsCustom)
        {
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
            Direction.X += MouseHandler.MouseDelta.X;
            Direction.Y += MouseHandler.MouseDelta.Y;
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
            MainGame.Forward = Utilities.ForwardVector(Direction.X * Utilities.PI180, Direction.Y * Utilities.PI180);
            // Keyboard based movement.
            Location oldvel = Velocity;
            Location movement = Location.Zero;
            if (!IsCustom)
            {
                left = KeyHandler.KeyBindIsDown(KeyBind.LEFT);
                right = KeyHandler.KeyBindIsDown(KeyBind.RIGHT);
                forward = KeyHandler.KeyBindIsDown(KeyBind.FORWARD);
                back = KeyHandler.KeyBindIsDown(KeyBind.BACK);
                up = KeyHandler.KeyBindIsDown(KeyBind.UP);
                down = KeyHandler.KeyBindIsDown(KeyBind.DOWN);
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
                Velocity = movement * 30;
            }
            else
            {
                if (movement.LengthSquared() > 0)
                {
                    movement = Utilities.RotateVector(movement, Direction.X * Utilities.PI180);
                }
                if (down)
                {
                    Velocity.Z = 0;
                    Position.Z = 20;
                }
                if (up)
                {
                    if (Velocity.Z < 0.1f && Velocity.Z > -0.1f
                        && Collision.Box(Position, new Location(-1.5f, -1.5f, -0.5f), new Location(1.5f, 1.5f, 2)))
                    {
                        Velocity.Z = 20;
                    }
                }
                Velocity = new Location(movement.X * 30, movement.Y * 30, Velocity.Z);
                Velocity.Z -= 100 * MyDelta;
            }
            Location target = Position + (oldvel + Velocity) * 0.5f * MyDelta;
            float pZ = Position.Z;
            Position = Collision.SlideBox(Position, target, new Location(-1.5f, -1.5f, 0), new Location(1.5f, 1.5f, 8));
            if (!ClientCVar.g_noclip.ValueB)
            {
                Velocity.Z = (Position.Z - pZ) / MyDelta;
            }
            if (!IsCustom)
            {
                byte move = MovementPacketOut.GetControlByte(forward, back, left, right, up, down);
                reps++;
                if (move != lastMove || Direction != lastdir || reps > 10)
                {
                    lastMove = move;
                    lastdir = Location.Zero;
                    reps = 0;
                    if (NetworkBase.IsActive)
                    {
                        Points.Add(CPoint());
                        if (Points.Count > 60)
                        {
                            Points.RemoveAt(0);
                        }
                        NetworkBase.Send(new MovementPacketOut(MainGame.GlobalTickTime, move, Direction.X, Direction.Y));
                    }
                }
            }
        }

        public BroadcastPoint CPoint()
        {
            return new BroadcastPoint(MainGame.GlobalTickTime, Position, Velocity, Direction, forward, back, left, right, up, down);
        }

        List<BroadcastPoint> Points = new List<BroadcastPoint>();

        public void ApplyMovement(Location Pos, Location Vel, double Time)
        {
            Location RealDir = Direction;
            for (int i = Points.Count - 1; i >= 0; i--)
            {
                if (Points[i].Time < Time)
                {
                    double TargetToMove = Time - Points[i].Time;
                    Position = Points[i].Position;
                    Velocity = Points[i].Velocity;
                    Points[i].Apply(this);
                    Update((float)TargetToMove, true);
                    Position += (Pos - Position);
                    Velocity += (Vel - Velocity);
                    double ctime = Time;
                    for (int x = i + 1; x < Points.Count; x++)
                    {
                        Points[i].Apply(this);
                        Update((float)(Points[i].Time - ctime), true);
                        ctime = Points[i].Time;
                    }
                    CPoint().Apply(this);
                    Update((float)(MainGame.GlobalTickTime - ctime), true);
                    break;
                }
            }
            // Backup - ensure Direction doesn't get messed up
            Direction = RealDir;
        }

        int reps = 0;
        byte lastMove = 0;
        Location lastdir = Location.Zero;
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
        public BroadcastPoint(double _time, Location _position, Location _velocity, Location _direction,
            bool _forward, bool _back, bool _left, bool _right, bool _up, bool _down)
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
        }
    }
}

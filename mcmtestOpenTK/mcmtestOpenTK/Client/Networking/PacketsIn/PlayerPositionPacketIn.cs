using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GameplayHandlers.Entities;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    public class PlayerPositionPacketIn: AbstractPacketIn
    {
        ulong eID;
        public Location position;
        public Location velocity;
        public Location direction;
        public ushort movement;
        public double Time;

        public override void FromBytes(byte[] input)
        {
            if (input.Length != 54)
            {
                IsValid = false;
                return;
            }
            eID = BitConverter.ToUInt64(input, 0);
            position = Location.FromBytes(input, 8);
            velocity = Location.FromBytes(input, 20);
            direction = Location.FromBytes(input, 32);
            movement = BitConverter.ToUInt16(input, 44);
            Time = BitConverter.ToDouble(input, 46);
            IsValid = true;
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            Entity ent = MainGame.GetEntity(eID);
            if (ent == null)
            {
                UIConsole.WriteLine("Invalid move packet (bad ID), supposed player " + eID + " moves to " + position.ToString());
            }
            else if (ent.Type != EntityType.PLAYER)
            {
                UIConsole.WriteLine("Invalid move packet (Not a player), supposed player " + eID + " moves to " + position.ToString() + " is actually " + ent.Type);
            }
            else
            {
                OtherPlayer player = (OtherPlayer)ent;
                if (Time > MainGame.GlobalTickTime)
                {
                    if (Time > MainGame.GlobalTickTime + 0.5)
                    {
                        return;
                    }
                    player.PacketsToApply.Add(this);
                    return;
                }
                if (Time < player.LastMovement)
                {
                    return;
                }
                player.ApplyNewMovement(Time, this);
            }
        }

        public static void ApplyPosition(OtherPlayer player, ushort movement, double yaw, double pitch)
        {
            player.Forward = (movement & 1) == 1;
            player.Back = (movement & 2) == 2;
            player.Left = (movement & 4) == 4;
            player.Right = (movement & 8) == 8;
            player.Up = (movement & 16) == 16;
            player.Down = (movement & 32) == 32;
            player.Slow = (movement & 64) == 64;
            player.Direction.X = yaw;
            player.Direction.Y = pitch;
        }
    }
}

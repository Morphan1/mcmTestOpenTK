using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    public class MovementPacketIn : AbstractPacketIn
    {
        public double Time;
        public ushort movement;
        public float yaw;
        public float pitch;

        public override void FromBytes(Player player, byte[] input)
        {
            if (input.Length != 18)
            {
                IsValid = false;
            }
            else
            {
                Time = BitConverter.ToDouble(input, 0);
                yaw = BitConverter.ToSingle(input, 8);
                if (yaw < 0 || yaw > 360)
                {
                    IsValid = false;
                    return;
                }
                pitch = BitConverter.ToSingle(input, 12);
                if (pitch < -89.9f || pitch > 89.9f)
                {
                    IsValid = false;
                    return;
                }
                movement = BitConverter.ToUInt16(input, 16);
                IsValid = true;
            }
        }

        public override void Execute(Player player)
        {
            if (!IsValid)
            {
                return;
            }
            if (Time > Server.GlobalTickTime)
            {
                if (Time > Server.GlobalTickTime + 1)
                {
                    // TODO: if (debug)
                    SysConsole.Output(OutputType.WARNING, player.Username +
                        " is cheating?: moving in the future ~ " + (float)Time + " vs " + (float)Server.GlobalTickTime);
                    player.Send(new TimePacketOut(true));
                    return;
                }
                player.PacketsToApply.Add(this);
                return;
            }
            if (Time < player.LastMovement)
            {
                if (Time < player.LastMovement - 1)
                {
                    // TODO: if (debug)
                    SysConsole.Output(OutputType.WARNING, player.Username +
                        " is cheating?: moving in the past ~ " + (float)Time + " vs " + (float)player.LastMovement);
                    player.Send(new TimePacketOut(true));
                }
                return;
            }
            player.ApplyNewMovement(Time, this);
        }

        public static void ApplyPosition(Player player, ushort movement, float yaw, float pitch)
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

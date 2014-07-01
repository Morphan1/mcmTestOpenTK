﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsIn
{
    public class MovementPacketIn : AbstractPacketIn
    {
        public double Time;
        public byte movement;
        public float yaw;
        public float pitch;

        public override void FromBytes(Player player, byte[] input)
        {
            if (input.Length != 17)
            {
                IsValid = false;
            }
            else
            {
                Time = BitConverter.ToDouble(input, 0);
                yaw = BitConverter.ToSingle(input, 8);
                pitch = BitConverter.ToSingle(input, 12);
                movement = input[16];
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
                if (Time > Server.GlobalTickTime + 0.5)
                {
                    SysConsole.Output(OutputType.WARNING, player.Username +
                        " is cheating?: moving in the future ~ " + (float)Time + " vs " + (float)Server.GlobalTickTime);
                    return;
                }
                player.PacketsToApply.Add(this);
                return;
            }
            if (Time < player.LastMovement)
            {
                if (Time < player.LastMovement - 0.5)
                {
                    SysConsole.Output(OutputType.WARNING, player.Username +
                        " is cheating?: moving in the past ~ " + (float)Time + " vs " + (float)player.LastMovement);
                }
                return;
            }
            player.ApplyNewMovement(Time, this);
        }

        public static void ApplyPosition(Player player, byte movement, float yaw, float pitch)
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

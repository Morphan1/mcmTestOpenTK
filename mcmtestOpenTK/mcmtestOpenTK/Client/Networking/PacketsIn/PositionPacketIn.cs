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
    class PositionPacketIn: AbstractPacketIn
    {
        ulong eID;
        Location position;
        Location velocity;
        Location direction;

        public override void FromBytes(byte[] input)
        {
            if (input.Length != 44)
            {
                IsValid = false;
                return;
            }
            eID = BitConverter.ToUInt64(input, 0);
            position = Location.FromBytes(input, 8);
            velocity = Location.FromBytes(input, 20);
            direction = Location.FromBytes(input, 32);
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
                UIConsole.WriteLine("Invalid move packet (bad ID), entity " + eID + " moves to " + position.ToString());
            }
            else
            {
                ent.Position = position;
                // TODO: Separate packet for position / movement
                if (ent is MovingEntity)
                {
                    ((MovingEntity)ent).Velocity = velocity;
                    ((MovingEntity)ent).Direction = direction;
                }
            }
        }
    }
}

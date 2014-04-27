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
using OpenTK;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class PositionPacketIn: AbstractPacketIn
    {
        ulong eID;
        Vector3 position;

        public override void FromBytes(byte[] input)
        {
            if (input.Length != 20)
            {
                IsValid = false;
                return;
            }
            eID = BitConverter.ToUInt64(input, 0);
            float X = BitConverter.ToSingle(input, 8);
            float Y = BitConverter.ToSingle(input, 12);
            float Z = BitConverter.ToSingle(input, 16);
            position = new Vector3(X, Y, Z);
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
            }
        }
    }
}

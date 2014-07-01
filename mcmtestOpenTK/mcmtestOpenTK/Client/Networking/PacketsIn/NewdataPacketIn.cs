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

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class NewdataPacketIn: AbstractPacketIn
    {
        ulong eID;
        byte[] data;

        public override void FromBytes(byte[] input)
        {
            if (input.Length < 8)
            {
                IsValid = false;
                return;
            }
            eID = BitConverter.ToUInt64(input, 0);
            data = new byte[input.Length - 8];
            Array.Copy(input, 8, data, 0, input.Length - 8);
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
                UIConsole.WriteLine("Invalid newdata packet (bad ID), entity " + eID + "!");
            }
            else
            {
                ent.ReadBytes(data);
            }
        }
    }
}

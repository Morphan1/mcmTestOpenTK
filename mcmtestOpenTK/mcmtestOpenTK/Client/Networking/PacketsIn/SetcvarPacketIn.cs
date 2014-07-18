using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommandHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.Networking.PacketsOut;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class SetcvarPacketIn: AbstractPacketIn
    {
        string CVar;

        string Value;

        public override void FromBytes(byte[] input)
        {
            if (input.Length > 4)
            {
                IsValid = true;
                CVar = NetStringManager.GetStringForID(BitConverter.ToInt32(input, 0));
                Value = FileHandler.encoding.GetString(input, 4, input.Length - 4);
            }
            else
            {
                IsValid = false;
            }
        }

        public override void Execute()
        {
            if (!IsValid)
            {
                return;
            }
            CVar cv = ClientCommands.Output.CVarSys.Get(CVar);
            if (cv == null || !cv.Flags.HasFlag(CVarFlag.ServerControl))
            {
                // Silently die ~ might be mod-made flag
                return;
            }
            cv.Set(Value, true);
        }
    }
}

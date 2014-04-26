using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.Networking.PacketsOut
{
    public class IdentityPacketOut: AbstractPacketOut
    {
        string Username;
        string Session;

        public IdentityPacketOut(string _name, string _session)
        {
            ID = 3;
            Username = _name;
            Session = _session;
        }

        public override byte[] ToBytes()
        {
            return FileHandler.encoding.GetBytes(Username + "\n" + Session);
        }
    }
}

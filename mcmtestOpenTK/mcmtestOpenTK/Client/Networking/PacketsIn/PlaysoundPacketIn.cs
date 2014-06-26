using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.AudioHandlers;

namespace mcmtestOpenTK.Client.Networking.PacketsIn
{
    class PlaysoundPacketIn: AbstractPacketIn
    {
        string soundname;

        public override void FromBytes(byte[] input)
        {
            if (input.Length == 4)
            {
                soundname = NetStringManager.GetStringForID(BitConverter.ToInt32(input, 0));
                IsValid = true;
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
            Sound sound = Sound.GetSound(soundname);
            sound.Play();
        }
    }
}

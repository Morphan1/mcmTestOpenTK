using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace mcmtestOpenTK.Client.AudioHandlers
{
    class SimpleAudioTest
    {
        public static FMOD.System FModSystem = null;
        public static List<FMOD.Channel> FModChannels = new List<FMOD.Channel>();
        public static FMOD.Sound TestS;

        /// <summary>
        /// Inits and loads a test sound
        /// </summary>
        public static void LoadSound()
        {
            // General Init
            FMOD.Factory.System_Create(ref FModSystem);
            FModSystem.init(32, FMOD.INITFLAGS.NORMAL, (IntPtr)null);

            // Load a sound
            FMOD.CREATESOUNDEXINFO exinfo = new FMOD.CREATESOUNDEXINFO();

            byte[] audiodata = File.ReadAllBytes(Environment.CurrentDirectory + "/test.ogg");

            exinfo.cbsize = Marshal.SizeOf(exinfo);
            exinfo.length = (uint)audiodata.Length;

            FMOD.RESULT res = FModSystem.createSound(audiodata, (FMOD.MODE.HARDWARE | FMOD.MODE.OPENMEMORY), ref exinfo, ref TestS);
        }

        /// <summary>
        /// Plays a test sound
        /// </summary>
        public static void PlayTestSound()
        {
            FMOD.Channel fmc = new FMOD.Channel();
            FModChannels.Add(fmc);
            FModSystem.playSound(FMOD.CHANNELINDEX.FREE, TestS, false, ref fmc);
        }

        /// <summary>
        /// Called every tick, is needed to maintain FMods audio channel list.
        /// </summary>
        public static void RecalculateChannels()
        {
            for (int i = 0; i < FModChannels.Count; i += 1)
            {
                FMOD.Channel chan = FModChannels[i];
                bool ispl = false;
                chan.isPlaying(ref ispl);
                if (!ispl)
                {
                    chan.stop();
                    FModChannels.Remove(chan);
                }
            }
        }
    }
}

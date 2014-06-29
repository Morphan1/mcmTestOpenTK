using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Shared;
using System.Runtime.InteropServices;

namespace mcmtestOpenTK.Client.AudioHandlers
{
    public class Sound
    {
        static FMOD.System FModSystem = null;
        static List<FMOD.Channel> FModChannels = new List<FMOD.Channel>();

        /// <summary>
        /// A full list of currently loaded textures.
        /// </summary>
        public static List<Sound> LoadedSounds = null;

        /// <summary>
        /// A default click sound.
        /// </summary>
        public static Sound Click = null;

        /// <summary>
        /// A default song sound.
        /// </summary>
        public static Sound Song = null;

        public static bool SystemLoaded = false;

        /// <summary>
        /// Starts or restarts the sound system.
        /// </summary>
        public static void InitSoundSystem()
        {
            // TODO: Linux compatible sound system!
#if WINDOWS
            // Dispose existing sounds
            if (LoadedSounds != null)
            {
                for (int i = 0; i < LoadedSounds.Count; i++)
                {
                    LoadedSounds[i].Remove();
                    i--;
                }
            }
            // Reset sound list
            LoadedSounds = new List<Sound>();
            // Load FMod System
            FMOD.Factory.System_Create(ref FModSystem);
            FModSystem.init(32, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
            // Preload a few common sounds
            Click = GenerateClick();
            LoadedSounds.Add(Click);
            SystemLoaded = true;
            Song = GetSound("common/song");
#else
            Click = new Sound();
#endif
        }

        #region clicknonsense
        static string ClickNonsense = "T2dnUwACAAAAAAAAAABKFQAAAAAAAGzVSE8BHgF2b3JiaXMAAAAAAoA+AAD/////aocAAP////+qAU9nZ1MAAAAAAAAAAAAAShUAAAEAAAASLpuBDz7/////////////////" +
            "4wN2b3JiaXMuAAAAQlM7IExhbmNlck1vZChTU0UpIChiYXNlZCBvbiBhb1R1ViBbMjAxMTA0MjRdKQAAAAABBXZvcmJpcyRCQ1YBAAgAAIAgChnGgNCQVQAAEAAAQohGxlCnlASXgoUQR8RQh5DzUGrpIHhKYcm" +
            "Y9BRrEEII33vPvffeeyA0ZBUAAAQAQBgFDmLgMQlCCKEYxQlRnCkIQghhOQmWch46CUL3IIQQLufecu699x4IDVkFAAACADAIIYQQQgghhBBCCimlFFKKKaaYYsoxxxxzzDHIIIMMOuikk04yqaSTjjLJqKPUWk" +
            "otxRRTbLnFWGutNefca1DKGGOMMcYYY4wxxhhjjDHGCEJDVgEAIAAAhEEGGWQQQgghhRRSiimmHHPMMceA0JBVAAAgAIAAAAAAR5EUyZEcyZEkSbIkS9Ikz/Isz/IsTxM1UVNFVXVV27V925d923d12bd92XZ1W" +
            "ZdlWXdtW5d1V9d1Xdd1Xdd1Xdd1Xdd1Xdd1IDRkFQAgAQCgIzmOIzmOIzmSIymSAoSGrAIAZAAABADgKI7iOJIjOZZjSZakSZrlWZ7laZ4maqIHhIasAgAAAQAEAAAAAACgKIriKI4jSZalaZrnqZ4oiqaqqqJp" +
            "qqqqmqZpmqZpmqZpmqZpmqZpmqZpmqZpmqZpmqZpmqZpmqZpmkBoyCoAQAIAQMdxHMdRHMdxHMmRJAkIDVkFAMgAAAgAwFAUR5Ecy7EkzdIsz/I00TM9V5RN3dRVGwgNWQUAAAIACAAAAAAAwPEcz/EcT/Ikz/I" +
            "cz/EkT9I0TdM0TdM0TdM0TdM0TdM0TdM0TdM0TdM0TdM0TdM0TdM0TdM0TdM0TdOA0JBVAAACAAAgiEKGMSA0ZBUAAAQAgBCikTHUKSXBpWAhxBEx1CHkPJRaOgieUlgyJj3FGoQQwvfec++99x4IDVkFAAABAB" +
            "BGgYMYeEyCEEIoRnFCFGcKghBCWE6CpZyHToLQPQghhMu5t5x7770HQkNWAQCAAAAMQgghhBBCCCGEkEJKKYWUYooppphyzDHHHHMMMsgggw466aSTTCrppKNMMuootZZSSzHFFFtuMdZaa8059xqUMsYYY4wxx" +
            "hhjjDHGGGOMMYLQkFUAAAgAAGGQQQYZhBBCSCGFlGKKKcccc8wxIDRkFQAACAAgAAAAwFEkRXIkR3IkSZIsyZI0ybM8y7M8y9NETdRUUVVd1XZt3/Zl3/ZdXfZtX7ZdXdZlWdZd29Zl3dV1Xdd1Xdd1Xdd1Xdd1" +
            "Xdd1HQgNWQUASAAA6EiO40iO40iO5EiKpAChIasAABkAAAEAOIqjOI7kSI7lWJIlaZJmeZZneZqniZroAaEhqwAAQAAAAQAAAAAAKIqiOIrjSJJlaZrmeaoniqKpqqpomqqqqqZpmqZpmqZpmqZpmqZpmqZpmqZ" +
            "pmqZpmqZpmqZpmqZpmiYQGrIKAJAAANBxHMdxFMdxHEdyJEkCQkNWAQAyAAACADAUxVEkx3IsSbM0y7M8TfRMzxVlUzd11QZCQ1YBAIAAAAIAAAAAAHA8x3M8x5M8ybM8x3M8yZM0TdM0TdM0TdM0TdM0TdM0TdM" +
            "0TdM0TdM0TdM0TdM0TdM0TdM0TdM0TdM0IDRkJQBABgCAGNIgcxRaA8hizEmKxRhjjDHGeEo8CKnVIioRmYPUiqbEY4xBCp4TkSnlKJhSXOgYtCJz0TGVlIstxhjjezGC0JAVAkBoBoDBcQBJ0wBJ0wAAAAAAAAB" +
            "A8jzAE01AE00AAAAAAAAAkDwP0EQR0EQRAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADJ8wDPNAHPNAEAAAAAAABAM01AFF3AdFUAAAA" +
            "AAAAA0EwTEF0TMFUXAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADJ8wDPNAHPNAEAAAAAAABAM03AdFVANF0AAAAAAAAA0EwTMFUXEF0" +
            "RAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
            "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
            "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
            "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
            "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
            "AAAAAAAAAAAAAAAAAAABAAABAgAMAQICFUGjIigAgTgDA4DiWBQAAjmVpFgAAOJZlWQAAYFmWKAIAgGVZoggAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
            "AAAAAAAAAIAAAYMABACDAhDJQaMhKACAKAMCgGJoHsCyAZQE0DaBpAM8DeB5AFAGAAACAAgcAgAAbNCUWByg0ZCUAEAUAYFAUy7Isz4OmaZrnQdM0zfOgaZ4nitA0zxNFeKLnmSY8z/NME6YpiqYJRNE0BQAAFDg" +
            "AAATYoCmxOEChISsBgJAAAIPiWJameZ7niaJpqio0zfNEURRN01RVFZbleaIoiqapqqoKTfM8URRF01RV14WmeZ4oiqJpqqrrwvNE0TRNU1Vd13XheaJomqapqq7ryhBFUTRN01RV15VlYJqmaZqq6rqyDETRNFX" +
            "VdWVZloEomqaquq4syzYwTVNVVdeVZdkGmKaquq4s2zZAVV1XlmXZtgGq6rquLNu6DXBd15Vl2bZ1AK4ry7Zs2wIAAA4cAAACjKCTjCqLsNGECw9AoSErAoAoAADAGKUUU8owRqWkVBrGpJRUSiUlpZRSqSCk1lI" +
            "IFZTUWgolo5RSarFVUFIpLcZKQioltVgAANiBAwDYgYVQaMhKACAPAAAgRinGnHNOSsmYcw5CKKVUzDnnoJNSMuYchBBKKRlzDkIHpZTOQQghhJRS5yCEEEpJKYQQQgglpVRSCCGUkFIqqZQQSkkppRRCCKUUAAB" +
            "U4AAAEGCjyOYEI0GFhqwEAFIBAAyOo2maplmeZ5qWZHme53meJ5qmZlme53me53mmyfM8T/REUTRNk+h5nih6niiaJlf1PFEURdNUTa7seaIpiqqquvA8zzNFV3ZteJ4nmqbryjZkWRRVFRts2zRd1bVtG6iqLNu" +
            "ybQNXll3Ztm0BAOAJDgBABTasjnBSNBZYaMhKACADAAAwBCHGmFKMMYQYY0oxxpQSAAAw4AAAEGBCGSg0ZEUAEAUAADjnnHPOOeecc84555xzzjnnnHPOMcYYY4wxxhhjjDHGGGOMMcYYY4wxxhhjjDHGGGOMCQD" +
            "YiXAA2ImwEAoNWQkAhAMAAAghBCmVUkopJVNKKSmllFJKKZlSSkkopZRSSikZc1BKKaWUUkrpmJQSSimllFJKKaWUUkoppZRSSimllFJKKaWUUkoppZRSSimllFJKKaWUUkoppZRSSimllFJKKaWUUkoppZRSSim" +
            "llFJKKaWUUkoppZRSSimllFJKKaWUUkoppZRSSimllFJKKaWUAgBMHhwAoBJsnGEl6axwNLjQkJUAQG4AAIAQc45BCK2l1kpJrbXUWgcdg1JSKqmVVlprqaXQOSihg9JaSi2V1FJrHYRQUkstpZRaS6m1lELoIKQ" +
            "QSkgppZZSaa2FlkpKKbXWUkqttNZKCSWUEkIopZWUQkolpVRKCaGUEEpKJZVUUioppRJKCSWEFEIqJaVSUkkdpFRCSamkVEpJJZSSQikhlVJKKimFVFIpKZVSQikppVBKKSWVUkIpqZRSSiiplFJSSqWUUlJJJZV" +
            "SQkqllJRKKaWUUlIKpZRUUiklhZJKKiWUUlJJpZRSUioplZJSCaGUUkpIoZSSUimlpJRKCKWkUkopKaVSSkoplFJKKKGkkkpJqaSUUiqplBJSKimllFJKqaRSSkmlpFBKKQAA6MABACDAiEoLsdOMK4/AEYUME1A" +
            "AACAIADAQITOBQAEUGMgAgAOEBCkAoLDAULrQBSFEkC6CLB64cOLGEzec0KENADAQITMBQjFESMgGgAmKCukAYHGBUbrQBSFEkC6CLB64cOLGEzec0KEFBAAAAACABwA+AAASDCAgIpq5DI0Njg6PD5AQkRESAAB" +
            "AAAEAAAAAAAAAAARPZ2dTAAT0AQAAAAAAAEoVAAACAAAA1YVpfgKCQ6qDl+o/ts0L+hhTHbxU/7FtXtDHGKt+wNgB/7caJUpS+grIOA7DOA/TOA/TOGfqnKlzps6ZzCazyWzQpwBAubu7u7u7u7u7u7u7u7u7u7u" +
            "7u7u7u7u7u7u7u7u7u7u7u7u7uyuKoih0Xdf74ODg4ODg4ODg4ODg4ODg4ODg4ODg4CCye3fwf7pvOaYjtXB27w7+T/ctx3SkFub0AzoGAACAB4Doa3l3PVdzdQ7OyWwwiorqnAAA5hTsRu88pTCnfXR0dDQ8";
        #endregion

        /// <summary>
        /// Generates a default click sound.
        /// </summary>
        /// <returns>The click sound object</returns>
        static Sound GenerateClick()
        {
            FMOD.Sound ClickSound = null;
            FMOD.CREATESOUNDEXINFO exinfo = new FMOD.CREATESOUNDEXINFO();
            byte[] audiodata = Convert.FromBase64String(ClickNonsense);
            exinfo.cbsize = Marshal.SizeOf(exinfo);
            exinfo.length = (uint)audiodata.Length;
            FMOD.RESULT res = FModSystem.createSound(audiodata, (FMOD.MODE.HARDWARE | FMOD.MODE.OPENMEMORY), ref exinfo, ref ClickSound);
            Sound sound = new Sound();
            sound.Name = "click";
            sound.LoadedProperly = true;
            sound.InternalSound = ClickSound;
            sound.Original_InternalSound = sound.InternalSound;
            return sound;
        }

        /// <summary>
        /// Gets the sound object for a specific sound name.
        /// </summary>
        /// <param name="texturename">The name of the sound</param>
        /// <returns>A valid sound object</returns>
        public static Sound GetSound(string soundname)
        {
            soundname = FileHandler.CleanFileName(soundname);
            for (int i = 0; i < LoadedSounds.Count; i++)
            {
                if (LoadedSounds[i].Name == soundname)
                {
                    return LoadedSounds[i];
                }
            }
            Sound Loaded = LoadSound(soundname);
            if (Loaded == null)
            {
                Loaded = new Sound();
                Loaded.Name = soundname;
                Loaded.InternalSound = Click.Original_InternalSound;
                Loaded.InternalSound = Click.Original_InternalSound;
                Loaded.LoadedProperly = false;
            }
            LoadedSounds.Add(Loaded);
            return Loaded;
        }

        /// <summary>
        /// Loads a sound from file.
        /// </summary>
        /// <param name="filename">The name of the file to use</param>
        /// <returns>The loaded sound, or null if it does not exist</returns>
        public static Sound LoadSound(string filename)
        {
            if (!SystemLoaded)
            {
                return new Sound() { Name = filename };
            }
            try
            {
                filename = FileHandler.CleanFileName(filename);
                if (!FileHandler.Exists("sounds/" + filename + ".ogg"))
                {
                    ErrorHandler.HandleError("Cannot load sound, file '" +
                        TextStyle.Color_Standout + "sounds/" + filename + ".ogg" + TextStyle.Color_Error +
                        "' does not exist.");
                    return null;
                }
                Sound sound = new Sound();
                sound.Name = filename;
                FMOD.Sound ClickSound = null;
                FMOD.CREATESOUNDEXINFO exinfo = new FMOD.CREATESOUNDEXINFO();
                byte[] audiodata = FileHandler.ReadBytes("sounds/" + filename + ".ogg");
                exinfo.cbsize = Marshal.SizeOf(exinfo);
                exinfo.length = (uint)audiodata.Length;
                FMOD.RESULT res = FModSystem.createSound(audiodata, (FMOD.MODE.HARDWARE | FMOD.MODE.OPENMEMORY), ref exinfo, ref ClickSound);
                sound.InternalSound = ClickSound;
                sound.Original_InternalSound = sound.InternalSound;
                sound.LoadedProperly = true;
                return sound;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Failed to load sound from filename '" +
                    TextStyle.Color_Standout + "sounds/" + filename + ".ogg" + TextStyle.Color_Error + "'", ex);
                return null;
            }
        }

        /// <summary>
        /// Called every tick, is needed to maintain FMods audio channel list.
        /// </summary>
        public static void RecalculateChannels()
        {
            if (!SystemLoaded)
            {
                return;
            }
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

        /// <summary>
        /// The full name of the sound.
        /// </summary>
        public string Name;

        /// <summary>
        /// The sound that this sound was remapped to, if any.
        /// </summary>
        public Sound RemappedTo;

        /// <summary>
        /// The internal FMod sound object to use.
        /// </summary>
        public FMOD.Sound InternalSound;

        /// <summary>
        /// The original internal FMod sound object that was used.
        /// </summary>
        public FMOD.Sound Original_InternalSound;

        /// <summary>
        /// Whether the sound loaded properly.
        /// </summary>
        public bool LoadedProperly = false;

        /// <summary>
        /// Removes the sound from the system.
        /// </summary>
        public void Remove()
        {
            // Dispose?
            LoadedSounds.Remove(this);
        }

        /// <summary>
        /// Plays the sound effect.
        /// </summary>
        public void Play()
        {
            if (!SystemLoaded)
            {
                return;
            }
            FMOD.Channel fmc = new FMOD.Channel();
            FModChannels.Add(fmc);
            FModSystem.playSound(FMOD.CHANNELINDEX.FREE, InternalSound, false, ref fmc);

        }
    }
}

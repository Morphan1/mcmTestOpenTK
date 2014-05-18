using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.AudioCmds
{
    public class SoundlistCommand: AbstractCommand
    {
        public SoundlistCommand()
        {
            Name = "soundlist";
            Arguments = "";
            Description = "Shows a list of all loaded sounds.";
            IsDebug = true;
            // TODO: Search argument.
        }

        public override void Execute(CommandEntry entry)
        {
            entry.Output.Good("There are <{color.emphasis}>" + Sound.LoadedSounds.Count + "<{color.base}> loaded sounds.");
            for (int i = 0; i < Sound.LoadedSounds.Count; i++)
            {
                entry.Output.Good("- <{color.emphasis}>" + TagParser.Escape(Sound.LoadedSounds[i].Name) +
                    (Sound.LoadedSounds[i].RemappedTo != null ? "<{color.simple}> -> <{color.emphasis}>" + TagParser.Escape(Sound.LoadedSounds[i].RemappedTo.Name): ""));
            }
            entry.Output.Good("-------");
        }
    }
}

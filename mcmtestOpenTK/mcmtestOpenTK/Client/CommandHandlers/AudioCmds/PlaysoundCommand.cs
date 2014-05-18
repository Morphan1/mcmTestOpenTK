using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.AudioCmds
{
    public class PlaysoundCommand : AbstractCommand
    {
        public PlaysoundCommand()
        {
            Name = "playsound";
            Arguments = "<sound to play>";
            Description = "Plays a sound file.";
            IsDebug = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                Sound sound = Sound.GetSound(entry.GetArgument(0));
                sound.Play();
                entry.Good("Playing sound '<{color.emphasis}>" + TagParser.Escape(sound.Name) + "<{color.base}>'.");
            }
        }
    }
}

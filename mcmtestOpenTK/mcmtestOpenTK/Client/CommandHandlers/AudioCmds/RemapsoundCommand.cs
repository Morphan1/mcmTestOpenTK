using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.AudioHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.AudioCmds
{
    public class RemapsoundCommand: AbstractCommand
    {
        public RemapsoundCommand()
        {
            Name = "remapsound";
            Arguments = "<sound to remap> <sound to remap to>";
            Description = "Replaces one sound with another, for all audible purposes.";
            IsDebug = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
            }
            else
            {
                Sound start = Sound.GetSound(entry.GetArgument(0));
                Sound target = Sound.GetSound(entry.GetArgument(1));
                start.InternalSound = target.Original_InternalSound;
                start.RemappedTo = target;
                entry.Good("Remapped sound '<{color.emphasis}>" + TagParser.Escape(start.Name) +
                    "<{color.base}>' to sound '<{color.emphasis}>" + TagParser.Escape(target.Name) + "<{color.base}>'.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.GraphicsCmds
{
    public class RemaptextureCommand: AbstractCommand
    {
        public RemaptextureCommand()
        {
            Name = "remaptexture";
            Arguments = "<texture to remap> <texture to remap to>";
            Description = "Replaces one texture with another, for all visual purposes.";
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
                Texture start = Texture.GetTexture(entry.GetArgument(0));
                Texture target = Texture.GetTexture(entry.GetArgument(1));
                start.Internal_Texture = target.Original_InternalID;
                start.RemappedTo = target;
                entry.Good("Remapped texture '<{color.emphasis}>" + TagParser.Escape(start.Name) +
                    "<{color.base}>' to texture '<{color.emphasis}>" + TagParser.Escape(target.Name) + "<{color.base}>'.");
            }
        }
    }
}

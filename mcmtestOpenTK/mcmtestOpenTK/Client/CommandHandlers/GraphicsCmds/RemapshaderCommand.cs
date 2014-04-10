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
    public class RemapshaderCommand: AbstractCommand
    {
        public RemapshaderCommand()
        {
            Name = "remapshader";
            Arguments = "<shader to remap> <shader to remap to>";
            Description = "Replaces one shader with another, for all visual purposes.";
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
                Shader start = Shader.GetShader(entry.GetArgument(0));
                Shader target = Shader.GetShader(entry.GetArgument(1));
                start.Internal_Program = target.Original_Program;
                start.RemappedTo = target;
                entry.Good("Remapped shader '<{color.emphasis}>" + TagParser.Escape(start.Name) +
                    "<{color.base}>' to shader '<{color.emphasis}>" + TagParser.Escape(target.Name) + "<{color.base}>'.");
            }
        }
    }
}

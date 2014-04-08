using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.UIHandlers;

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

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 2)
            {
                ShowUsage(info);
            }
            else
            {
                Shader start = Shader.GetShader(info.GetArgument(0));
                Shader target = Shader.GetShader(info.GetArgument(1));
                start.Internal_Program = target.Original_Program;
                start.RemappedTo = target;
                UIConsole.WriteLine(TextStyle.Color_Outgood + "Remapped shader '" + TextStyle.Color_Separate + start.Name +
                    TextStyle.Color_Outgood + "' to shader '" + TextStyle.Color_Separate + target.Name + TextStyle.Color_Outgood + "'.");
            }
        }
    }
}

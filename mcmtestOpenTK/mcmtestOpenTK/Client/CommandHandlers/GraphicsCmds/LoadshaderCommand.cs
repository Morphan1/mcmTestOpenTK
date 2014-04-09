using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.GraphicsCmds
{
    public class LoadshaderCommand : AbstractCommand
    {
        public LoadshaderCommand()
        {
            Name = "loadshader";
            Arguments = "<shader to load>";
            Description = "Loads a shader file.";
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
                Shader shader = Shader.GetShader(entry.GetArgument(0));
                if (shader.LoadedProperly)
                {
                    UIConsole.WriteLine(TextStyle.Color_Outgood + "Successfully loaded shader '" + TextStyle.Color_Separate +
                        shader.Name + TextStyle.Color_Outgood + "'.");
                }
                else
                {
                    UIConsole.WriteLine(TextStyle.Color_Outbad + "Failed to load shader '" + TextStyle.Color_Separate +
                        shader.Name + TextStyle.Color_Outbad + "'.");
                }
            }
        }
    }
}

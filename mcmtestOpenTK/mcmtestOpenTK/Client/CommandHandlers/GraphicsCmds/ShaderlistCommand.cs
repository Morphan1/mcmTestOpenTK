using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Client.CommandHandlers.GraphicsCmds
{
    public class ShaderlistCommand : AbstractCommand
    {
        public ShaderlistCommand()
        {
            Name = "shaderlist";
            Arguments = "";
            Description = "Shows a list of all loaded shaders.";
            IsDebug = true;
            // TODO: Search argument.
        }

        public override void Execute(CommandEntry entry)
        {
            UIConsole.WriteLine(TextStyle.Color_Outgood + "There are " + TextStyle.Color_Separate +
                Shader.LoadedShaders.Count + TextStyle.Color_Outgood + " loaded shaders.");
            UIConsole.WriteLine(TextStyle.Color_Separate + "OriginalID" + TextStyle.Color_Simple + ") [" + 
                TextStyle.Color_Separate + "CurrentID" + TextStyle.Color_Simple + "] " + TextStyle.Color_Separate + "Name"
                + TextStyle.Color_Simple + " -> " + TextStyle.Color_Separate + "RemappedName");
            for (int i = 0; i < Shader.LoadedShaders.Count; i++)
            {
                UIConsole.WriteLine("- " + TextStyle.Color_Separate + Shader.LoadedShaders[i].Original_Program +
                    TextStyle.Color_Simple + ") [" + TextStyle.Color_Separate + Shader.LoadedShaders[i].Internal_Program +
                    TextStyle.Color_Simple + "] " + TextStyle.Color_Separate + Shader.LoadedShaders[i].Name +
                    (Shader.LoadedShaders[i].Internal_Program != Shader.LoadedShaders[i].Original_Program ?
                    TextStyle.Color_Simple + " -> " + TextStyle.Color_Separate + Shader.LoadedShaders[i].RemappedTo.Name : ""));
            }
            UIConsole.WriteLine(TextStyle.Color_Outgood + "-------");
        }
    }
}

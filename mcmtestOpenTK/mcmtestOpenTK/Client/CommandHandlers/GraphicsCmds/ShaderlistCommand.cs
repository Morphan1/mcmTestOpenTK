using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

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
            entry.Output.Good("There are <{color.emphasis}>" + Shader.LoadedShaders.Count + "<{color.base}> loaded shaders.");
            entry.Output.Good("<{color.emphasis}>OriginalID<{color.simple}>) [<{color.emphasis}>CurrentID<{color.simple}>] " +
                "<{color.emphasis}>Name<{color.simple}> -> <{color.emphasis}>RemappedName");
            for (int i = 0; i < Shader.LoadedShaders.Count; i++)
            {
                entry.Output.Good("- <{color.emphasis}>" + Shader.LoadedShaders[i].Original_Program + "<{color.simple}>) [<{color.emphasis}>" +
                    Shader.LoadedShaders[i].Internal_Program + "<{color.simple}>] <{color.emphasis}>" + TagParser.Escape(Shader.LoadedShaders[i].Name) +
                    (Shader.LoadedShaders[i].Internal_Program != Shader.LoadedShaders[i].Original_Program ?
                    "<{color.simple}> -> <{color.emphasis}>" + TagParser.Escape(Shader.LoadedShaders[i].RemappedTo.Name): ""));
            }
            entry.Output.Good("-------");
        }
    }
}

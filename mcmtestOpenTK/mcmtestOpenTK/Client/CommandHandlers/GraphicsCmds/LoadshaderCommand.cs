using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

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
                    entry.Good("Successfully loaded shader '<{color.emphasis}>" + TagParser.Escape(shader.Name) + "<{color.base}>'.");
                }
                else
                {
                    entry.Bad("Failed to load shader '<{color.emphasis}>" + TagParser.Escape(shader.Name) + "<{color.base}>'.");
                }
            }
        }
    }
}

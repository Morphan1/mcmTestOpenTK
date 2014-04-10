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
    public class LoadtextureCommand : AbstractCommand
    {
        public LoadtextureCommand()
        {
            Name = "loadtexture";
            Arguments = "<texture to load>";
            Description = "Loads a texture file.";
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
                Texture texture = Texture.GetTexture(entry.GetArgument(0));
                if (texture.LoadedProperly)
                {
                    entry.Good("Successfully loaded texture '<{color.emphasis}>" + TagParser.Escape(texture.Name) + "<{color.base}>'.");
                }
                else
                {
                    entry.Bad("Failed to load texture '<{color.emphasis}>" + TagParser.Escape(texture.Name) + "<{color.base}>'.");
                }
            }
        }
    }
}

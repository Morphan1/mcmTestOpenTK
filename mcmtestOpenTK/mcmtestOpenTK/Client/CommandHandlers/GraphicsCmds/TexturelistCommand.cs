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
    public class TexturelistCommand: AbstractCommand
    {
        public TexturelistCommand()
        {
            Name = "texturelist";
            Arguments = "";
            Description = "Shows a list of all loaded textures.";
            IsDebug = true;
            // TODO: Search argument.
        }

        public override void Execute(CommandEntry entry)
        {
            entry.Output.Good("There are <{color.emphasis}>" + Texture.LoadedTextures.Count + "<{color.base}> loaded textures.");
            entry.Output.Good("<{color.emphasis}>OriginalID<{color.simple}>) [<{color.emphasis}>CurrentID<{color.simple}>] <{color.emphasis}>" +
                "Width<{color.simple}>*<{color.emphasis}>Height<{color.simple}>: <{color.emphasis}>Name<{color.simple}> -> <{color.emphasis}>RemappedName");
            for (int i = 0; i < Texture.LoadedTextures.Count; i++)
            {
                entry.Output.Good("- <{color.emphasis}>" + Texture.LoadedTextures[i].Original_InternalID + "<{color.simple}>) [<{color.emphasis}>" +
                    Texture.LoadedTextures[i].Internal_Texture + "<{color.simple}>] <{color.emphasis}>" + Texture.LoadedTextures[i].Width +
                    "<{color.simple}>*<{color.emphasis}>" + Texture.LoadedTextures[i].Height + "<{color.simple}>: <{color.emphasis}>" +
                    TagParser.Escape(Texture.LoadedTextures[i].Name) + (Texture.LoadedTextures[i].Internal_Texture != Texture.LoadedTextures[i].Original_InternalID ?
                    "<{color.simple}> -> <{color.emphasis}>" + TagParser.Escape(Texture.LoadedTextures[i].RemappedTo.Name): ""));
            }
            entry.Output.Good("-------");
        }
    }
}

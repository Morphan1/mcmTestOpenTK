using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.CommandHandlers.Graphics
{
    public class TexturelistCommand: AbstractCommand
    {
        public TexturelistCommand()
        {
            Name = "texturelist";
            Arguments = "";
            Description = "Shows a list of all loaded textures.";
            IsDebug = true;
        }

        public override void Execute(CommandInfo info)
        {
            UIConsole.WriteLine(TextStyle.Color_Outgood + "There are " + TextStyle.Color_Separate +
                Texture.LoadedTextures.Count + TextStyle.Color_Outgood + " loaded textures.");
            UIConsole.WriteLine(TextStyle.Color_Separate + "OriginalID" + TextStyle.Color_Simple + ") [" + 
                TextStyle.Color_Separate + "CurrentID" + TextStyle.Color_Simple + "] " + TextStyle.Color_Separate +
                "Width" + TextStyle.Color_Simple + "*" + TextStyle.Color_Separate + "Height" + TextStyle.Color_Simple +
                ": " + TextStyle.Color_Separate + "Name"
                + TextStyle.Color_Simple + " -> " + TextStyle.Color_Separate + "RemappedName");
            for (int i = 0; i < Texture.LoadedTextures.Count; i++)
            {
                UIConsole.WriteLine("- " + TextStyle.Color_Separate + Texture.LoadedTextures[i].Original_InternalID +
                    TextStyle.Color_Simple + ") [" + TextStyle.Color_Separate + Texture.LoadedTextures[i].Internal_Texture +
                    TextStyle.Color_Simple + "] " + TextStyle.Color_Separate + Texture.LoadedTextures[i].Width + TextStyle.Color_Simple +
                    "*" + TextStyle.Color_Separate + Texture.LoadedTextures[i].Height + TextStyle.Color_Simple + ": " +
                    TextStyle.Color_Separate + Texture.LoadedTextures[i].Name +
                    (Texture.LoadedTextures[i].Internal_Texture != Texture.LoadedTextures[i].Original_InternalID ?
                    TextStyle.Color_Simple + " -> " + TextStyle.Color_Separate + Texture.LoadedTextures[i].RemappedTo.Name : ""));
            }
            UIConsole.WriteLine(TextStyle.Color_Outgood + "-------");
        }
    }
}

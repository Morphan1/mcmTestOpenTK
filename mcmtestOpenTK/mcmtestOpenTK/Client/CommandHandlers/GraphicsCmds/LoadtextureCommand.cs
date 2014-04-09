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
                    UIConsole.WriteLine(TextStyle.Color_Outgood + "Successfully loaded texture '" + TextStyle.Color_Separate +
                        texture.Name + TextStyle.Color_Outgood + "'.");
                }
                else
                {
                    UIConsole.WriteLine(TextStyle.Color_Outbad + "Failed to load texture '" + TextStyle.Color_Separate +
                        texture.Name + TextStyle.Color_Outbad + "'.");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.GraphicsCmds
{
    public class SavetextureCommand: AbstractCommand
    {
        public SavetextureCommand()
        {
            Name = "savetexture";
            Arguments = "<texture to save> <filename to save to>";
            Description = "Saves a texture to disk.";
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
                Texture start = Texture.GetTexture(entry.GetArgument(0));
                string fname = entry.GetArgument(1);
                try
                {
                    start.SaveToFile(fname);
                    entry.Good("Saved texture '<{color.emphasis}>" + TagParser.Escape(start.Name) +
                        "<{color.base}>' to file '<{color.emphasis}>" + TagParser.Escape(fname) + "<{color.base}>");
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("Commands/Savetexture", ex);
                    entry.Bad("Error saving texture '<{color.emphasis}>" + TagParser.Escape(start.Name) +
                        "<{color.base}>' to file '<{color.emphasis}>" + TagParser.Escape(fname) + "<{color.base}>'.");
                }
            }
        }
    }
}

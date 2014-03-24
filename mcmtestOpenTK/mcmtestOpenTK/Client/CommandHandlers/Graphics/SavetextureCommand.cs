﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommonHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.Graphics
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

        public override void Execute(CommandInfo info)
        {
            if (info.Arguments.Count < 2)
            {
                ShowUsage(info);
            }
            else
            {
                Texture start = Texture.GetTexture(info.Arguments[0]);
                string fname = info.Arguments[1];
                try
                {
                    start.SaveToFile(fname);
                    UIConsole.WriteLine(TextStyle.Color_Outgood + "Saved texture '" + TextStyle.Color_Separate + start.Name +
                        TextStyle.Color_Outgood + "' to file '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outgood + "'.");
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError(ex);
                    UIConsole.WriteLine(TextStyle.Color_Outbad + "Error saving texture '" + TextStyle.Color_Separate + start.Name +
                        TextStyle.Color_Outbad + "' to file '" + TextStyle.Color_Separate + fname + TextStyle.Color_Outbad + "'.");
                }
            }
        }
    }
}
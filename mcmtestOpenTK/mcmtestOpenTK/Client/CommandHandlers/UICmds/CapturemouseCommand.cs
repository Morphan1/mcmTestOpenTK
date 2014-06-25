using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;
using OpenTK.Input;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class CapturemouseCommand : AbstractCommand
    {
        public CapturemouseCommand()
        {
            Name = "capturemouse";
            Arguments = "";
            Description = "Captures or releases the mouse.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (MouseHandler.MouseCaptured)
            {
                MouseHandler.ReleaseMouse();
                entry.Good("Mouse released.");
            }
            else
            {
                MouseHandler.CaptureMouse();
                entry.Good("Mouse captured.");
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class WaitCommand: AbstractCommand
    {
        public WaitCommand()
        {
            Name = "wait";
            Arguments = "<time to wait in seconds>";
            Description = "Delays the current command queue a specified amount of time.";
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string delay = entry.GetArgument(0);
                float seconds = Utilities.StringToFloat(delay);
                if (entry.Queue.Delayable)
                {
                    entry.Good("Delaying for <{color.emphasis}>" + seconds + "<{color.base}> seconds.");
                    entry.Queue.Wait = seconds;
                }
                else
                {
                    entry.Bad("Cannot delay, inside an instant queue!");
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class ParsingCommand: AbstractCommand
    {
        public ParsingCommand()
        {
            Name = "parsing";
            Arguments = "on/off";
            Description = "Sets whether the current queue should parse tags.";
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
                bool modechoice = entry.GetArgument(0).ToLower() == "on";
                entry.Queue.ParseTags = modechoice;
                entry.Good("Queue parsing <{color.emphasis}>" + (modechoice ? "enabled" : "disabled") + "<{color.base}>.");
            }
        }
    }
}

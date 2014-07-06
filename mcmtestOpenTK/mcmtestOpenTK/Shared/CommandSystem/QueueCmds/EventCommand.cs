﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class EventCommand : AbstractCommand
    {
        public EventCommand()
        {
            Name = "event";
            Arguments = "add/remove/clear <name of event>/all [name of event handler] (quiet_fail)";
            Description = "Creates a new function of the following command block, and adds it to the specified event's handler.";
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
                return;
            }
            string type = entry.GetArgument(0).ToLower();
            string eventname = entry.GetArgument(1).ToLower();
            if (type == "clear" && eventname == "all")
            {
                foreach (KeyValuePair<string, ScriptEvent> evt in entry.Queue.CommandSystem.Events)
                {
                    evt.Value.Handlers.Clear();
                }
                entry.Good("Cleared all events.");
                return;
            }
            ScriptEvent theEvent;
            if (!entry.Queue.CommandSystem.Events.TryGetValue(eventname, out theEvent))
            {
                entry.Bad("Unknown event '<{color.emphasis}>" + TagParser.Escape(eventname) + "<{color.base}>'.");
                return;
            }
            if (type == "clear")
            {
                int count = theEvent.Handlers.Count;
                theEvent.Handlers.Clear();
                entry.Good("Cleared <{color.emphasis}>" + count + "<{color.base}> event handler" + (count == 1 ? "." : "s."));
            }
            else if (type == "remove")
            {
                if (entry.Arguments.Count < 3)
                {
                    ShowUsage(entry);
                    return;
                }
                string name = entry.GetArgument(2).ToLower();
                bool success = false;
                for (int i = 0; i < theEvent.Handlers.Count; i++)
                {
                    if (theEvent.Handlers[i].Name == "eventhandler_" + theEvent.Name + "_" + name)
                    {
                        theEvent.Handlers.RemoveAt(i);
                        success = true;
                        break;
                    }
                }
                if (success)
                {
                    entry.Good("Removed event handler '<{color.emphasis}>" + TagParser.Escape(name) + "<{color.base}>'.");
                }
                else
                {
                    if (entry.Arguments.Count > 3 && entry.GetArgument(3).ToLower() == "quiet_fail")
                    {
                        entry.Good("Unknown event handler '<{color.emphasis}>" + TagParser.Escape(name) + "<{color.base}>'.");
                    }
                    else
                    {
                        entry.Bad("Unknown event handler '<{color.emphasis}>" + TagParser.Escape(name) + "<{color.base}>'.");
                    }
                }
            }
            else if (type == "add")
            {
                if (entry.Arguments.Count < 3)
                {
                    ShowUsage(entry);
                    return;
                }
                string name = entry.GetArgument(2).ToLower();
                if (entry.Block == null)
                {
                    entry.Bad("Event command invalid: No block follows!");
                    return;
                }
                bool success = false;
                for (int i = 0; i < theEvent.Handlers.Count; i++)
                {
                    if (theEvent.Handlers[i].Name == "eventhandler_" + theEvent.Name + "_" + name)
                    {
                        theEvent.Handlers.RemoveAt(i);
                        success = true;
                        break;
                    }
                }
                if (success)
                {
                    if (entry.Arguments.Count > 1 && entry.GetArgument(1).ToLower() == "quiet_fail")
                    {
                        entry.Good("Handler '<{color.emphasis}>" + TagParser.Escape(name) + "<{color.base}>' already exists!");
                    }
                    else
                    {
                        entry.Bad("Handler '<{color.emphasis}>" + TagParser.Escape(name) + "<{color.base}>' already exists!");
                    }
                }
                else
                {
                    entry.Good("Handler '<{color.emphasis}>" + TagParser.Escape(name) +
                        "<{color.base}>' defined for event '<{color.emphasis}>" + TagParser.Escape(theEvent.Name) + "<{color.base}>'.");
                    theEvent.Handlers.Add(new CommandScript("eventhandler_" +
                        theEvent.Name + "_" + name, CommandScript.DisOwn(entry.Block, entry)) { Debug = DebugMode.MINIMAL });
                }
            }
            else
            {
                ShowUsage(entry);
            }
        }
    }
}

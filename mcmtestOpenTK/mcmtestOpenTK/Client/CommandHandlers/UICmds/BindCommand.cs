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
    class BindCommand : AbstractCommand
    {
        public BindCommand()
        {
            Name = "bind";
            Arguments = "<key to bind> [script to bind to it]";
            Description = "Binds a script to a key.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count == 1)
            {
                string key = entry.GetArgument(0);
                Key targetkey = KeyHandler.GetKeyForName(key);
                if (targetkey != Key.Unknown)
                {
                    CommandScript script = KeyHandler.GetBind(targetkey);
                    if (script != null)
                    {
                        entry.Info("Key '<{color.emphasis}>" + TagParser.Escape(key.ToLower()) +
                            "<{color.base}>' is bound to \"" + script.FullString() + "\".");
                    }
                    else
                    {
                        entry.Info("Key '<{color.emphasis}>" + TagParser.Escape(key.ToLower()) + "<{color.base}>' is unbound.");
                    }
                }
                else
                {
                    entry.Bad("Unknown key '<{color.emphasis}>" + TagParser.Escape(key.ToLower()) + "<{color.base}>'.");
                }
                return;
            }
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
            }
            else
            {
                string key = entry.GetArgument(0);
                string bind = entry.AllArguments(1);
                Key targetkey = KeyHandler.GetKeyForName(key);
                if (targetkey != Key.Unknown)
                {
                    KeyHandler.BindKey(targetkey, bind);
                    entry.Good("Bound key <{color.emphasis}>" + TagParser.Escape(key.ToLower()) + "<{color.base}>.");
                }
                else
                {
                    entry.Bad("Unknown key '<{color.emphasis}>" + TagParser.Escape(key.ToLower()) + "<{color.base}>'.");
                }
            }
        }
    }
}

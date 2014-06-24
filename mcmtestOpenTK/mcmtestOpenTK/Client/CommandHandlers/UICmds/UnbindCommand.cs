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
    class UnbindCommand : AbstractCommand
    {
        public UnbindCommand()
        {
            Name = "unbind";
            Arguments = "<key to unbind>";
            Description = "Unbinds a key from any scripts.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string key = entry.GetArgument(0);
                Key targetkey = KeyHandler.GetKeyForName(key);
                if (targetkey != Key.Unknown)
                {
                    KeyHandler.BindKey(targetkey, null);
                    entry.Good("Unbound key <{color.emphasis}>" + TagParser.Escape(key.ToLower()) + "<{color.base}>.");
                }
                else
                {
                    entry.Bad("Unknown key '<{color.emphasis}>" + TagParser.Escape(key.ToLower()) + "<{color.base}>'.");
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Shared.CommandSystem.QueueCmds
{
    class IfCommand: AbstractCommand
    {
        public IfCommand()
        {
            Name = "if";
            Arguments = "<true/false>";
            Description = "Executes the following block of commands only if the input is true.";
            IsFlow = true;
        }

        public override void Execute(CommandEntry entry)
        {
            entry.Result = 0;
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                if (entry.Arguments[0] == "\0CALLBACK")
                {
                    return;
                }
                if (entry.Block == null)
                {
                    entry.Bad("If invalid: No block follows!");
                    return;
                }
                string comparison = entry.AllArguments();
                bool success = TryIf(comparison);
                if (success)
                {
                    entry.Good("If is true, executing...");
                    entry.Result = 1;
                    entry.Block.Add(new CommandEntry("if \0CALLBACK", null, entry,
                        this, new List<string> { "\0CALLBACK" }, "if", 0));
                    entry.Queue.AddCommandsNow(entry.Block);
                }
                else
                {
                    entry.Good("If is false, doing nothing!");
                }
            }
        }

        public static bool TryIf(string info)
        {
            info = "(" + info.ToLower().Replace("&&", "&").Replace("||", "|").Replace(" ", "") + ")";
            int mark = -1;
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i] == '(')
                {
                    mark = i;
                }
                if (info[i] == ')' && i != -1 && i - (mark + 1) > 0)
                {
                    info = (mark > 0 ? info.Substring(0, mark): "") +
                        TryAnds(info.Substring(mark + 1, i - (mark + 1))) +
                        (i + 1 < info.Length ? info.Substring(i + 1): "");
                    i = -1;
                    mark = -1;
                }
            }
            return TextIsTrue(info);
        }

        static string TryAnds(string info)
        {
            if (info.Length == 0)
            {
                return "false";
            }
            if (TextIsTrue(info))
            {
                return "true";
            }
            string[] ands = info.Split('&');
            for (int i = 0; i < ands.Length; i++)
            {
                if (!TryOrs(ands[i]))
                {
                    return "false";
                }
            }
            return "true";
        }

        static bool TryOrs(string info)
        {
            if (TextIsTrue(info))
            {
                return true;
            }
            string[] ors = info.Split('|');
            for (int i = 0; i < ors.Length; i++)
            {
                if (TextIsTrue(ors[i]))
                {
                    return true;
                }
            }
            return false;
        }

        static bool TextIsTrue(string info)
        {
            return info == "true";
        }
    }
}

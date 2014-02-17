using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.UIHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers
{
    public class Commands
    {
        /// <summary>
        /// Seperates a string list of command inputs (separated by newlines, semicolons, ...)
        /// and returns a list of the individual commands.
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        /// <returns>A list of command strings</returns>
        public static List<string> SeparateCommands(string commands)
        {
            if (!commands.EndsWith("\n"))
            {
                commands = commands + "\n";
            }
            List<string> CommandList = new List<string>();
            int start = 0;
            bool quoted = false;
            for (int i = 0; i < commands.Length; i++)
            {
                if (commands[i] == '"')
                {
                    quoted = !quoted;
                }
                else if ((commands[i] == '\n') || (!quoted && commands[i] == ';'))
                {
                    if (i - start > 0)
                    {
                        CommandList.Add(commands.Substring(start, i - start));
                    }
                    start = i + 1;
                    quoted = false;
                }
            }
            return CommandList;
        }

        /// <summary>
        /// Executes an arbitrary list of command inputs (separated by newlines, semicolons, ...)
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        public static void ExecuteCommands(string commands)
        {
            List<string> ParsedCmds = SeparateCommands(commands);
            for (int i = 0; i < ParsedCmds.Count; i++)
            {
                ExecuteCommand(ParsedCmds[i]);
            }
        }

        /// <summary>
        /// Executes a single command.
        /// </summary>
        /// <param name="command">The command string to execute</param>
        public static void ExecuteCommand(string command)
        {
            try
            {
                UIConsole.WriteLine(TextStyle.Color_Chat + "Executing command '" + TextStyle.Color_Standout + command + TextStyle.Color_Chat + "'.");
                throw new Exception("Potato!");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Command '" + TextStyle.Color_Standout + command + TextStyle.Color_Error + "' threw an error", ex);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommandHandlers.Common;
using mcmtestOpenTK.Client.CommandHandlers.Graphics;
using mcmtestOpenTK.Client.CommandHandlers.Network;

namespace mcmtestOpenTK.Client.CommandHandlers
{
    public class Commands
    {
        /// <summary>
        /// A full list of all registered commands.
        /// </summary>
        public static List<AbstractCommand> RegisteredCommands;

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
                        CommandList.Add(commands.Substring(start, i - start).Trim());
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
                if (command.StartsWith("//"))
                {
                    return;
                }
                if (command.StartsWith("/"))
                {
                    command = command.Substring(1);
                }
                List<string> args = new List<string>();
                int start = 0;
                bool quoted = false;
                for (int i = 0; i < command.Length; i++)
                {
                    if (command[i] == '"')
                    {
                        quoted = !quoted;
                    }
                    else if (!quoted && command[i] == ' ' && (i - start > 0))
                    {
                        string arg = command.Substring(start, i - start).Trim().Replace("\"", "");
                        args.Add(arg);
                        start = i + 1;
                    }
                }
                if (command.Length - start > 0)
                {
                    string arg = command.Substring(start, command.Length - start).Trim().Replace("\"", "");
                    args.Add(arg);
                }
                if (args.Count == 0)
                {
                    return;
                }
                string BaseCommand = args[0];
                string BaseCommandLow = args[0].ToLower();
                args.RemoveAt(0);
                for (int i = 0; i < RegisteredCommands.Count; i++)
                {
                    if (BaseCommandLow == RegisteredCommands[i].Name)
                    {
                        CommandInfo info = new CommandInfo(BaseCommand, RegisteredCommands[i], args);
                        RegisteredCommands[i].Execute(info);
                        return;
                    }
                }
                UIConsole.WriteLine(TextStyle.Color_Error + "Unknown command '" +
                    TextStyle.Color_Standout + BaseCommand + TextStyle.Color_Error + "'.");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Command '" + TextStyle.Color_Standout + command + TextStyle.Color_Error + "' threw an error", ex);
            }
        }

        /// <summary>
        /// Adds a command to the registered command list.
        /// </summary>
        /// <param name="command">The command to register</param>
        public static void RegisterCommand(AbstractCommand command)
        {
            RegisteredCommands.Add(command);
        }

        /// <summary>
        /// Prepares the command system, registering all base commands.
        /// </summary>
        public static void Init()
        {
            RegisteredCommands = new List<AbstractCommand>();

            // Common
            RegisterCommand(new CvarinfoCommand());
            RegisterCommand(new EchoCommand());
            RegisterCommand(new HelpCommand());
            RegisterCommand(new HideconsoleCommand());
            RegisterCommand(new QuitCommand());
            RegisterCommand(new SetCommand());
            RegisterCommand(new ShowconsoleCommand());

            // Graphics
            RegisterCommand(new LoadshaderCommand());
            RegisterCommand(new LoadtextureCommand());
            RegisterCommand(new ReloadCommand());
            RegisterCommand(new RemapshaderCommand());
            RegisterCommand(new RemaptextureCommand());
            RegisterCommand(new ReplacefontCommand());
            RegisterCommand(new SavetextureCommand());
            RegisterCommand(new ShaderlistCommand());
            RegisterCommand(new TexturelistCommand());

            // Network
            RegisterCommand(new LoginCommand());
            RegisterCommand(new TimeCommand());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommandHandlers.CommonCmds;
using mcmtestOpenTK.Client.CommandHandlers.GraphicsCmds;
using mcmtestOpenTK.Client.CommandHandlers.NetworkCmds;
using mcmtestOpenTK.Client.CommandHandlers.QueueCmds;

namespace mcmtestOpenTK.Client.CommandHandlers
{
    public class Commands
    {
        /// <summary>
        /// A full list of all registered commands.
        /// </summary>
        public static List<AbstractCommand> RegisteredCommands;

        /// <summary>
        /// All command queues currently running.
        /// </summary>
        public static List<CommandQueue> Queues;

        /// <summary>
        /// Executes an arbitrary list of command inputs (separated by newlines, semicolons, ...)
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        public static void ExecuteCommands(string commands)
        {
            CommandQueue.SeparateCommands(commands).Execute();
        }

        /// <summary>
        /// Executes a single command.
        /// </summary>
        /// <param name="entry">The command entry to execute</param>
        /// <param name="queue">The queue that is executing it</param>
        /// <returns>The CommandInfo object made, if any</returns>
        public static CommandInfo ExecuteCommand(CommandEntry entry, CommandQueue queue)
        {
            string command = entry.Command;
            try
            {
                if (command.StartsWith("//"))
                {
                    return null;
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
                    return null;
                }
                string BaseCommand = args[0];
                string BaseCommandLow = args[0].ToLower();
                args.RemoveAt(0);
                for (int i = 0; i < RegisteredCommands.Count; i++)
                {
                    if (BaseCommandLow == RegisteredCommands[i].Name)
                    {
                        CommandInfo info = new CommandInfo(BaseCommand, RegisteredCommands[i], args, queue, entry);
                        entry.Info = info;
                        RegisteredCommands[i].Execute(info);
                        return info;
                    }
                }
                UIConsole.WriteLine(TextStyle.Color_Error + "Unknown command '" +
                    TextStyle.Color_Standout + BaseCommand + TextStyle.Color_Error + "'.");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Command '" + TextStyle.Color_Standout + command + TextStyle.Color_Error + "' threw an error", ex);
            }
            return null;
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
            Queues = new List<CommandQueue>();

            // Common
            RegisterCommand(new CvarinfoCommand());
            RegisterCommand(new EchoCommand());
            RegisterCommand(new HelpCommand());
            RegisterCommand(new HideconsoleCommand());
            RegisterCommand(new QuitCommand());
            RegisterCommand(new SetCommand());
            RegisterCommand(new ShowconsoleCommand());

            // Queue-related
            RegisterCommand(new ElseCommand());
            RegisterCommand(new IfCommand());
            RegisterCommand(new RunCommand());
            RegisterCommand(new StopCommand());
            RegisterCommand(new WaitCommand());

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

        /// <summary>
        /// Advances any running command queues.
        /// </summary>
        public static void Tick()
        {
            for (int i = 0; i < Queues.Count; i++)
            {
                Queues[i].Tick();
                if (!Queues[i].Running)
                {
                    Queues.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}

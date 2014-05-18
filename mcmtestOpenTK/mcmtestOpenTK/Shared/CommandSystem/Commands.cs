using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem.QueueCmds;
using mcmtestOpenTK.Shared.CommandSystem.CommonCmds;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public class Commands
    {
        /// <summary>
        /// A full list of all registered commands.
        /// </summary>
        public List<AbstractCommand> RegisteredCommands;

        /// <summary>
        /// All command queues currently running.
        /// </summary>
        public List<CommandQueue> Queues;

        /// <summary>
        /// The tag handling system.
        /// </summary>
        public TagParser TagSystem;

        /// <summary>
        /// The output system.
        /// </summary>
        public Outputter Output;

        /// <summary>
        /// Executes an arbitrary list of command inputs (separated by newlines, semicolons, ...)
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        public void ExecuteCommands(string commands)
        {
            CommandQueue.SeparateCommands(commands, this).Execute();
        }

        /// <summary>
        /// Executes a single command.
        /// </summary>
        /// <param name="entry">The command entry to execute</param>
        /// <param name="queue">The queue that is executing it</param>
        public void ExecuteCommand(CommandEntry entry, CommandQueue queue)
        {
            string command = entry.CommandLine;
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
                        entry.Name = BaseCommand;
                        entry.Command = RegisteredCommands[i];
                        entry.Arguments = args;
                        entry.Queue = queue;
                        entry.Output = Output;
                        RegisteredCommands[i].Execute(entry);
                        return;
                    }
                }
                Output.UnknownCommand(BaseCommandLow, args.ToArray());
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
        public void RegisterCommand(AbstractCommand command)
        {
            RegisteredCommands.Add(command);
        }

        /// <summary>
        /// Prepares the command system, registering all base commands.
        /// </summary>
        public void Init()
        {
            RegisteredCommands = new List<AbstractCommand>();
            Queues = new List<CommandQueue>();
            TagSystem = new TagParser();
            TagSystem.Init();

            // Common Commands
            RegisterCommand(new CvarinfoCommand());
            RegisterCommand(new EchoCommand());
            RegisterCommand(new SetCommand());

            // Queue-related Commands
            RegisterCommand(new DebugCommand());
            RegisterCommand(new ElseCommand());
            RegisterCommand(new IfCommand());
            RegisterCommand(new InsertCommand());
            RegisterCommand(new RepeatCommand());
            RegisterCommand(new RunCommand());
            RegisterCommand(new StopallCommand());
            RegisterCommand(new StopCommand());
            RegisterCommand(new WaitCommand());
        }

        /// <summary>
        /// Advances any running command queues.
        /// <param name="Delta">The time passed this tick</param>
        /// </summary>
        public void Tick(float Delta)
        {
            for (int i = 0; i < Queues.Count; i++)
            {
                Queues[i].Tick(Delta);
                if (!Queues[i].Running)
                {
                    Queues.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}

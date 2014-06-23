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
        /// A full dictionary of all registered commands.
        /// </summary>
        public Dictionary<string, AbstractCommand> RegisteredCommands;

        /// <summary>
        /// A full list of all registered commands.
        /// </summary>
        public List<AbstractCommand> RegisteredCommandList;

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
        /// The AbstractCommand for the invalid command-command.
        /// </summary>
        public DebugOutputInvalidCommand DebugInvalidCommand;

        /// <summary>
        /// All scripts this command system has loaded.
        /// </summary>
        public Dictionary<string, CommandScript> Scripts;

        /// <summary>
        /// Executes a command script.
        /// </summary>
        /// <param name="script">The script to execute</param>
        public void ExecuteScript(CommandScript script)
        {
            script.ToQueue(this).Execute();
        }

        /// <summary>
        /// Gets a script saved in the command system by name, or creates one from file.
        /// </summary>
        /// <param name="script">The name of the script</param>
        /// <returns>A script, or null if there's no match</returns>
        public CommandScript GetScript(string script)
        {
            CommandScript commandscript;
            if (Scripts.TryGetValue(script.ToLower(), out commandscript))
            {
                return commandscript;
            }
            else
            {
                commandscript = CommandScript.GetByFileName(script, this);
                if (commandscript != null)
                {
                    Scripts.Add(commandscript.Name, commandscript);
                }
                return commandscript;
            }
        }

        /// <summary>
        /// Executes an arbitrary list of command inputs (separated by newlines, semicolons, ...)
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        public void ExecuteCommands(string commands)
        {
            CommandScript.SeparateCommands("command_line", commands, this).ToQueue(this).Execute();
        }

        /// <summary>
        /// Executes a single command.
        /// </summary>
        /// <param name="entry">The command entry to execute</param>
        /// <param name="queue">The queue that is executing it</param>
        public void ExecuteCommand(CommandEntry entry, CommandQueue queue)
        {
            try
            {
                entry.Queue = queue;
                entry.Output = Output;
                entry.Command.Execute(entry);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Command '" + TextStyle.Color_Standout + entry.CommandLine + TextStyle.Color_Error + "' threw an error", ex);
            }
        }

        /// <summary>
        /// Adds a command to the registered command list.
        /// </summary>
        /// <param name="command">The command to register</param>
        public void RegisterCommand(AbstractCommand command)
        {
            RegisteredCommands.Add(command.Name, command);
            RegisteredCommandList.Add(command);
        }

        /// <summary>
        /// Prepares the command system, registering all base commands.
        /// </summary>
        public void Init()
        {
            RegisteredCommands = new Dictionary<string, AbstractCommand>(30);
            RegisteredCommandList = new List<AbstractCommand>(30);
            Scripts = new Dictionary<string, CommandScript>(30);
            Queues = new List<CommandQueue>(20);
            TagSystem = new TagParser();
            TagSystem.Init(this);

            // Common Commands
            RegisterCommand(new CvarinfoCommand());
            RegisterCommand(new EchoCommand());
            RegisterCommand(new SetCommand());

            // Queue-related Commands
            RegisterCommand(new DebugCommand());
            RegisterCommand(new DefineCommand());
            RegisterCommand(new ElseCommand());
            RegisterCommand(new IfCommand());
            RegisterCommand(new InsertCommand());
            RegisterCommand(new RepeatCommand());
            RegisterCommand(new RunCommand());
            RegisterCommand(new StopallCommand());
            RegisterCommand(new StopCommand());
            RegisterCommand(new WaitCommand());

            // Register debug command
            DebugInvalidCommand = new DebugOutputInvalidCommand();
            RegisterCommand(DebugInvalidCommand);
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

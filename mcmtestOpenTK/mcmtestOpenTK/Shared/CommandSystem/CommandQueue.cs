using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public class CommandQueue
    {
        /// <summary>
        /// Separates a string list of command inputs (separated by newlines, semicolons, ...)
        /// and returns a queue object containing all the input commands
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        /// <param name="system">The command system to put the queue in</param>
        /// <returns>A list of command strings</returns>
        public static CommandQueue SeparateCommands(string commands, Commands system)
        {
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
                    if (start < i)
                    {
                        CommandList.Add(commands.Substring(start, i - start).Trim());
                    }
                    start = i + 1;
                    quoted = false;
                }
            }
            if (start < commands.Length)
            {
                CommandList.Add(commands.Substring(start).Trim());
            }
            return new CommandQueue(CreateBlock(CommandList, null), system);
        }

        /// <summary>
        /// Seperates a string list of command inputs (separated by newlines, semicolons, ...)
        /// and returns a list of the individual commands.
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        /// <returns>A list of command strings</returns>
        public static List<CommandEntry> SeparateCommands(string commands)
        {
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
                    if (start < i)
                    {
                        CommandList.Add(commands.Substring(start, i - start).Trim());
                    }
                    start = i + 1;
                    quoted = false;
                }
            }
            if (start < commands.Length)
            {
                CommandList.Add(commands.Substring(start).Trim());
            }
            return CreateBlock(CommandList, null);
        }

        /// <summary>
        /// Converts a list of command strings to a CommandEntry list, handling any { braced } blocks inside.
        /// </summary>
        /// <param name="from">The command strings</param>
        /// <param name="entry">The entry that owns this block</param>
        /// <returns>A list of entries with blocks separated</returns>
        public static List<CommandEntry> CreateBlock(List<string> from, CommandEntry entry)
        {
            List<CommandEntry> toret = new List<CommandEntry>();
            List<string> Temp = null;
            int blocks = 0;
            for (int i = 0; i < from.Count; i++)
            {
                if (from[i] == "{")
                {
                    blocks++;
                    if (blocks == 1)
                    {
                        Temp = new List<string>();
                    }
                    else
                    {
                        Temp.Add("{");
                    }
                }
                else if (from[i] == "}")
                {
                    blocks--;
                    if (blocks == 0)
                    {
                        if (toret.Count == 0)
                        {
                            List<CommandEntry> block = CreateBlock(Temp, entry);
                            toret.AddRange(block);
                        }
                        else
                        {
                            List<CommandEntry> block = CreateBlock(Temp, toret[toret.Count - 1]);
                            toret[toret.Count - 1].Block = block;
                        }
                    }
                    else if (blocks < 0)
                    {
                        blocks = 0;
                        Temp.Add("echo \"" + TextStyle.Color_Error + "SCRIPT ERROR: EXTRA } SYMBOL!\"");
                    }
                    else
                    {
                        Temp.Add("}");
                    }
                }
                else if (blocks > 0)
                {
                    Temp.Add(from[i]);
                }
                else
                {
                    toret.Add(new CommandEntry(from[i], null, entry));
                }
            }
            return toret;
        }

        /// <summary>
        /// All commands in this queue, as strings.
        /// </summary>
        public List<CommandEntry> CommandList;

        /// <summary>
        /// A list of all variables saved in this queue.
        /// </summary>
        public List<Variable> Variables;

        /// <summary>
        /// Whether the queue can be delayed (EG, via a WAIT command).
        /// </summary>
        public bool Delayable = true;

        /// <summary>
        /// How long until the queue may continue.
        /// </summary>
        public float Wait = 0;

        /// <summary>
        /// Whether the queue is running.
        /// </summary>
        public bool Running = false;

        /// <summary>
        /// The last command to be run.
        /// </summary>
        public CommandEntry LastCommand;

        /// <summary>
        /// The command system running this queue.
        /// </summary>
        public Commands CommandSystem;

        public CommandQueue(List<CommandEntry> _commands, Commands _system)
        {
            CommandList = _commands;
            CommandSystem = _system;
            Variables = new List<Variable>();
        }

        /// <summary>
        /// Starts running the command queue.
        /// </summary>
        public void Execute()
        {
            if (Running)
            {
                return;
            }
            Running = true;
            Tick(0f);
            if (Running)
            {
                CommandSystem.Queues.Add(this);
            }
        }

        /// <summary>
        /// Recalculates and advances the command queue.
        /// <param name="Delta">The time passed this tick</param>
        /// </summary>
        public void Tick(float Delta)
        {
            if (Delayable && Wait > 0f)
            {
                Wait -= Delta;
                if (Wait > 0)
                {
                    return;
                }
                Wait = 0;
            }
            while (CommandList.Count > 0)
            {
                CommandEntry CurrentCommand = CommandList[0];
                CommandList.RemoveAt(0);
                CommandSystem.ExecuteCommand(CurrentCommand, this);
                LastCommand = CurrentCommand;
                if (Delayable && Wait > 0f)
                {
                    return;
                }
            }
            Running = false;
        }

        /// <summary>
        /// Adds a list of entries to be executed next in line.
        /// </summary>
        /// <param name="entries">Commands to be run</param>
        public void AddCommandsNow(List<CommandEntry> entries)
        {
            CommandList.InsertRange(0, entries);
        }

        /// <summary>
        /// Immediately stops the Command Queue.
        /// </summary>
        public void Stop()
        {
            CommandList.Clear();
        }

        /// <summary>
        /// Adds or sets a variable for tags in this queue to use.
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value to set on the variable</param>
        public void SetVariable(string name, string value)
        {
            string namelow = name.ToLower();
            for (int i = 0; i < Variables.Count; i++)
            {
                if (Variables[i].Name == namelow)
                {
                    Variables[i].Value = value;
                    return;
                }
            }
            Variables.Add(new Variable(namelow, value));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem.CommonCmds;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public class CommandEntry
    {
        /// <summary>
        /// Creates a CommandEntry from the given input and queue information.
        /// </summary>
        /// <param name="_command">The command line text itself</param>
        /// <param name="_block">The command block that held this entry</param>
        /// <param name="_owner">The command entry that owns the block that held this entry</param>
        /// <param name="system">The command system to work from</param>
        /// <returns>The command system</returns>
        public static CommandEntry FromInput(string command, List<CommandEntry> _block, CommandEntry _owner, Commands system)
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
            AbstractCommand cmd;
            if (system.RegisteredCommands.TryGetValue(BaseCommandLow, out cmd))
            {
                return new CommandEntry(command, _block, _owner, cmd, args, BaseCommand);
            }
            return CreateInvalidOutput(BaseCommand, _block, args, _owner, system);
        }

        public static CommandEntry CreateInvalidOutput(string name, List<CommandEntry> _block,
            List<string> _arguments, CommandEntry _owner, Commands system)
        {
            _arguments.Insert(0, name);
            return new CommandEntry("\0DebugOutputInvalidCommand \"" + name + "\"", _block, _owner,
                system.DebugInvalidCommand, _arguments, name);
                
        }

        /// <summary>
        /// The command itself.
        /// </summary>
        public string CommandLine;

        /// <summary>
        /// If the command has a block of { braced } commands, this will contain that block.
        /// </summary>
        public List<CommandEntry> Block;

        /// <summary>
        /// What command entry object owns this entry, if any.
        /// </summary>
        public CommandEntry BlockOwner = null;

        public CommandEntry(string _commandline, List<CommandEntry> _block, CommandEntry _owner,
            AbstractCommand _command, List<string> _arguments, string _name)
        {
            CommandLine = Utilities.CleanStringInput(_commandline);
            Block = _block;
            BlockOwner = _owner;
            Command = _command;
            Arguments = _arguments;
            Name = _name;
        }

        /// <summary>
        /// Use at own risk.
        /// </summary>
        public CommandEntry()
        {
        }

        /// <summary>
        /// The command name input by the user.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// The command that should execute this input.
        /// </summary>
        public AbstractCommand Command;

        /// <summary>
        /// The arguments input by the user.
        /// </summary>
        public List<string> Arguments;

        /// <summary>
        /// The command queue this command is running inside.
        /// </summary>
        public CommandQueue Queue = null;

        /// <summary>
        /// The object to use for any console / debug output.
        /// </summary>
        public Outputter Output = null;

        /// <summary>
        /// A result set by the command, if any.
        /// </summary>
        public int Result = 0;

        /// <summary>
        /// An index set by the command, if any.
        /// </summary>
        public int Index = 0;

        /// <summary>
        /// An object set by the command, if any.
        /// </summary>
        public Object obj = null;

        /// <summary>
        /// Gets an argument at a specified place, handling any tags.
        /// </summary>
        /// <param name="place">The argument place number</param>
        /// <returns>The parsed argument</returns>
        public string GetArgument(int place)
        {
            if (place >= Arguments.Count || place < 0)
            {
                throw new ArgumentOutOfRangeException("Value must be greater than 0 and less than command input argument count");
            }
            return Queue.CommandSystem.TagSystem.ParseTags(Arguments[place], TextStyle.Color_Simple, Queue.Variables, Queue.Debug);
        }

        /// <summary>
        /// Gets all arguments piled together into a string.
        /// </summary>
        /// <returns>The combined string</returns>
        public string AllArguments()
        {
            StringBuilder result = new StringBuilder(CommandLine.Length);
            for (int i = 0; i < Arguments.Count; i++)
            {
                result.Append(GetArgument(i)).Append(" ");
            }
            return result.ToString();
        }

        /// <summary>
        /// Used to output a success message.
        /// </summary>
        /// <param name="tagged_text">The text to output, with tags included</param>
        public void Good(string text)
        {
            if (Queue.Debug == DebugMode.FULL)
            {
                Output.Good(text, DebugMode.MINIMAL);
            }
        }

        /// <summary>
        /// Used to output a failure message.
        /// </summary>
        /// <param name="tagged_text">The text to output, with tags included</param>
        public void Bad(string text)
        {
            if (Queue.Debug <= DebugMode.MINIMAL)
            {
                Output.Bad(text, DebugMode.MINIMAL);
            }
        }

        /// <summary>
        /// Returns a duplicate of this command entry.
        /// </summary>
        /// <param name="NewOwner">The new owner of the command entry</param>
        /// <returns>The duplicate entry</returns>
        public CommandEntry Duplicate(CommandEntry NewOwner = null)
        {
            CommandEntry entry = new CommandEntry();
            entry.Arguments = new List<string>(Arguments);
            if (Block == null)
            {
                entry.Block = null;
            }
            else
            {
                entry.Block = new List<CommandEntry>();
                for (int i = 0; i < Block.Count; i++)
                {
                    entry.Block.Add(Block[i].Duplicate(entry));
                }
            }
            entry.BlockOwner = NewOwner;
            entry.Command = Command;
            entry.CommandLine = CommandLine;
            entry.Index = Index;
            entry.Name = Name;
            entry.Output = Output;
            entry.Queue = Queue;
            entry.Result = Result;
            entry.obj = obj;
            return entry;
        }
    }
}

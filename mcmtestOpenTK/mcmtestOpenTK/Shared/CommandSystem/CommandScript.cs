﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public class CommandScript
    {
        /// <summary>
        /// Separates a string list of command inputs (separated by newlines, semicolons, ...)
        /// and returns a queue object containing all the input commands
        /// </summary>
        /// <param name="name">The name of the script</param>
        /// <param name="commands">The command string to parse</param>
        /// <param name="system">The command system to create the script within</param>
        /// <returns>A list of command strings</returns>
        public static CommandScript SeparateCommands(string name, string commands, Commands system)
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
            return new CommandScript(name, CreateBlock(CommandList, null, system));
        }

        /// <summary>
        /// Converts a list of command strings to a CommandEntry list, handling any { braced } blocks inside.
        /// </summary>
        /// <param name="from">The command strings</param>
        /// <param name="entry">The entry that owns this block</param>
        /// <param name="system">The command system to create this block inside</param>
        /// <returns>A list of entries with blocks separated</returns>
        public static List<CommandEntry> CreateBlock(List<string> from, CommandEntry entry, Commands system)
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
                            List<CommandEntry> block = CreateBlock(Temp, entry, system);
                            toret.AddRange(block);
                        }
                        else
                        {
                            List<CommandEntry> block = CreateBlock(Temp, toret[toret.Count - 1], system);
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
                    toret.Add(CommandEntry.FromInput(from[i], null, entry, system));
                }
            }
            return toret;
        }

        /// <summary>
        /// Creates a script by file name.
        /// File is /scripts/filename.cfg
        /// </summary>
        /// <param name="filename">The name of the file to execute</param>
        /// <param name="system">The command system to get the script for</param>
        /// <returns>A command script, or null of the file does not exist</returns>
        public static CommandScript GetByFileName(string filename, Commands system)
        {
            try
            {
                string fname = "scripts/" + filename + ".cfg";
                if (FileHandler.Exists(fname))
                {
                    string text = FileHandler.ReadText(fname);
                    return SeparateCommands(filename, text, system);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Generating script for file '" + filename + "'", ex);
                return null;
            }
        }

        /// <summary>
        /// The name of the script.
        /// </summary>
        public string Name;

        /// <summary>
        /// All commands in the script.
        /// </summary>
        public List<CommandEntry> Commands;

        public CommandScript(string _name, List<CommandEntry> _commands)
        {
            Name = _name.ToLower();
            Commands = _commands;
        }

        /// <summary>
        /// Returns a duplicate of the script's entry list.
        /// </summary>
        /// <returns>The entry list</returns>
        public List<CommandEntry> GetEntries()
        {
            List<CommandEntry> entries = new List<CommandEntry>(Commands.Count);
            for (int i = 0; i < Commands.Count; i++)
            {
                entries.Add(Commands[i].Duplicate());
            }
            return entries;
        }

        /// <summary>
        /// Creates a new queue for this script.
        /// </summary>
        /// <param name="system">The command system to make the queue in</param>
        /// <returns>The created queue</returns>
        public CommandQueue ToQueue(Commands system)
        {
            return new CommandQueue(this, GetEntries(), system);
        }
    }
}

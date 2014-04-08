using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.CommandHandlers
{
    public class CommandQueue
    {
        /// <summary>
        /// Seperates a string list of command inputs (separated by newlines, semicolons, ...)
        /// and returns a list of the individual commands.
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        /// <returns>A list of command strings</returns>
        public static CommandQueue SeparateCommands(string commands)
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
            return new CommandQueue(CommandList);
        }

        /// <summary>
        /// All commands in this queue, as strings.
        /// </summary>
        public List<string> CommandList;

        /// <summary>
        /// Whether the queue can be delayed (EG, via a WAIT command).
        /// </summary>
        public bool Delayable = true;

        /// <summary>
        /// Where in the command list the queue is currently executing at.
        /// </summary>
        public int Spot = 0;

        /// <summary>
        /// How long until the queue may continue.
        /// </summary>
        public float Wait = 0;

        /// <summary>
        /// Whether the queue is running.
        /// </summary>
        public bool Running = false;

        public CommandQueue(List<string> _commands)
        {
            CommandList = _commands;
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
            Commands.Queues.Add(this);
            Tick();
        }

        /// <summary>
        /// Recalculates and advances the command queue.
        /// </summary>
        public void Tick()
        {
            if (Delayable && Wait > 0f)
            {
                Wait -= MainGame.DeltaF;
                if (Wait > 0)
                {
                    return;
                }
                Wait = 0;
            }
            for (; Spot < CommandList.Count; Spot++)
            {
                if (Delayable && Wait > 0f)
                {
                    return;
                }
                Commands.ExecuteCommand(CommandList[Spot], this);
            }
            Running = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.ServerSystem.PlayerCommands.CommonCmds;

namespace mcmtestOpenTK.ServerSystem.PlayerCommands
{
    public class PlayerCommandEngine
    {
        /// <summary>
        /// A full list of all registered commands.
        /// </summary>
        public static List<PlayerAbstractCommand> RegisteredCommands;

        /// <summary>
        /// Runs a command as a player.
        /// </summary>
        /// <param name="player">The player to run the command</param>
        /// <param name="arguments">The command and arguments to run</param>
        public static void RunCommand(Player player, List<string> arguments)
        {
            StringBuilder commandstring = new StringBuilder();
            commandstring.Append(arguments[0]);
            for (int i = 1; i < arguments.Count; i++)
            {
                commandstring.Append(" \"" + arguments[i] + "\"");
            }
            PlayerCommandEntry entry = new PlayerCommandEntry();
            entry.CommandLine = commandstring.ToString();
            entry.Name = arguments[0];
            entry.player = player;
            entry.Arguments = arguments;
            SysConsole.Output(OutputType.INFO, player.Username + " issued command: /" + entry.CommandLine);
            string cmdlow = entry.Name.ToLower();
            for (int i = 0; i < RegisteredCommands.Count; i++)
            {
                if (RegisteredCommands[i].Name == cmdlow)
                {
                    entry.Arguments.RemoveAt(0);
                    entry.Command = RegisteredCommands[i];
                    RegisteredCommands[i].Execute(entry);
                    return;
                }
            }
            SysConsole.Output(OutputType.INFO, "[Command Invalid / Unknown]");
            player.SendMessage(TextStyle.Color_Error + "Unknown command '" + TextStyle.Color_Separate + arguments[0] + TextStyle.Color_Error + "'.");
        }

        /// <summary>
        /// Adds a command to the registered command list.
        /// </summary>
        /// <param name="command">The command to register</param>
        public static void RegisterCommand(PlayerAbstractCommand command)
        {
            RegisteredCommands.Add(command);
        }

        /// <summary>
        /// Prepares the command system, registering all base commands.
        /// </summary>
        public static void Init()
        {
            RegisteredCommands = new List<PlayerAbstractCommand>();
            RegisterCommand(new BulletCommand());
            RegisterCommand(new SayCommand());
        }
    }
}

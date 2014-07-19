using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.ServerSystem.PlayerCommands.CommonCmds;
using mcmtestOpenTK.ServerSystem.CommandHandlers;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.ServerSystem.PlayerCommands
{
    public class PlayerCommandEngine
    {
        /// <summary>
        /// A full list of all registered commands.
        /// </summary>
        public static Dictionary<string, PlayerAbstractCommand> RegisteredCommands;

        /// <summary>
        /// A full list of all registered commands.
        /// TODO: FOR CLEAN HELP FILE OUTPUT
        /// </summary>
        public static List<PlayerAbstractCommand> RegisteredCommandsList;

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
            arguments.RemoveAt(0);
            entry.player = player;
            entry.Arguments = arguments;
            SysConsole.Output(OutputType.INFO, player.Username + " issued command: /" + entry.CommandLine);
            string cmdlow = entry.Name.ToLower();
            PlayerAbstractCommand cmd;
            bool cmdExists = RegisteredCommands.TryGetValue(cmdlow, out cmd);
            // <--[event]
            // @Name player command
            // @Group Commands
            // @Triggers When a player inputs a command to the server.
            // @Cancellable true
            // @Warning This event can potentially fire very rapidly if the client uses a script.
            // @Note This event won't fire for entirely clientside commands.
            // @Note This event can be used to override existing commands or create new ones.
            // Just listen for the command by name, cancel the event, and add your own handling.
            // @Variables
            // player: The username of the player that did the command.
            // command: The command that was input.
            // command_exists: whether the command is an existent registered command on the internal system.
            // arguments: a list of all input arguments.
            // -->
            Dictionary<string, string> EventVars = new Dictionary<string,string>();
            EventVars.Add("player", player.Username);
            EventVars.Add("command", cmdlow);
            EventVars.Add("command_exists", cmdExists ? "true": "false");
            EventVars.Add("arguments", new ListTag(arguments).ToString());
            bool cancelled = ServerCommands.PlayerCommandEvent.Call(EventVars);
            if (cmdExists && !cancelled)
            {
                if (player.HasPermission("commands." + cmd.Name))
                {
                    entry.Command = cmd;
                    cmd.Execute(entry);
                }
                else
                {
                    SysConsole.Output(OutputType.INFO, "[Command Refused / No Permission]");
                    player.SendMessage(TextStyle.Color_Error + "You do not have permission to use the command '" + TextStyle.Color_Separate + cmd.Name + TextStyle.Color_Error + "'.");
                }
            }
            else if (!cancelled)
            {
                SysConsole.Output(OutputType.INFO, "[Command Invalid / Unknown]");
                player.SendMessage(TextStyle.Color_Error + "Unknown command '" + TextStyle.Color_Separate + cmdlow + TextStyle.Color_Error + "'.");
            }
        }

        /// <summary>
        /// Adds a command to the registered command list.
        /// </summary>
        /// <param name="command">The command to register</param>
        public static void RegisterCommand(PlayerAbstractCommand command)
        {
            RegisteredCommands.Add(command.Name, command);
            RegisteredCommandsList.Add(command);
        }

        /// <summary>
        /// Prepares the command system, registering all base commands.
        /// </summary>
        public static void Init()
        {
            RegisteredCommands = new Dictionary<string, PlayerAbstractCommand>();
            RegisteredCommandsList = new List<PlayerAbstractCommand>();
            RegisterCommand(new BulletCommand());
            RegisterCommand(new ItemCommand());
            RegisterCommand(new SayCommand());
            RegisterCommand(new NoclipCommand());
        }
    }
}

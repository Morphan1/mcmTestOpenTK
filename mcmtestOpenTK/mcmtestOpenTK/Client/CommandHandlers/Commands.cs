using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Client.CommandHandlers.Common;
using mcmtestOpenTK.Client.CommandHandlers.Graphics;

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
                /*UIConsole.WriteLine("ABCDEFGHIJKLMNOPQRSTUVWXYZ\nabcdefghijklmnopqrstuvwxyz\n1234567890\nXX XX\tXX\nXX\tXX\nX\tXX\nX\tX\n" +
                                                               "TESTCOLORS^1RED^2GREEN^3THREE\n" +
                                                               "^r^7Text Colors: ^0^h^1^^n1 ^!^^n! ^2^^n2 ^@^^n@ ^3^^n3 ^#^^n# ^4^^n4 ^$^^n$ ^5^^n5 ^%^^n% ^6^^n6 ^-^^n- ^7^^n7 ^&^^n& ^8^^n8 ^*^^** ^9^^n9 ^(^^n( ^&^h^0^^n0^h ^)^^n) ^a^^na ^A^^nA\n" +
                                            "^7Text styles: ^b^^nb is bold,^r ^i^^ni is italic,^r ^u^^nu is underline,^r ^s^^ns is strike-through,^r ^O^^nO is overline,^r ^7^h^0^^nh is highlight,^r^7 ^j^^nj is jello (AKA jiggle), ^r\n" +
                                            "^2^e^0^^ne is emphasis,^r^7 ^t^^nt is transparent,^r ^T^^nT is more transparent,^r ^o^^no is opaque,^r ^R^^nR is random,^r ^p^^np is pseudo-random,^r ^^nk is obfuscated (^kobfu^r),^r\n" +
                                            "^^nS is ^SSuperScript^r, ^^nl is ^lSubScript (AKA Lower-Text)^r, ^h^8^d^^nd is Drop-Shadow,^r^7 ^f^^nf is flip,^r ^^nr is regular text, ^^nq is a ^qquote^q, and ^^nn is nothing (escape-symbol).");
                */
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
            RegisterCommand(new EchoCommand());

            // Graphics
            RegisterCommand(new LoadshaderCommand());
            RegisterCommand(new LoadtextureCommand());
            RegisterCommand(new RemapshaderCommand());
            RegisterCommand(new RemaptextureCommand());
            RegisterCommand(new SavetextureCommand());
            RegisterCommand(new ShaderlistCommand());
            RegisterCommand(new TexturelistCommand());
        }
    }
}

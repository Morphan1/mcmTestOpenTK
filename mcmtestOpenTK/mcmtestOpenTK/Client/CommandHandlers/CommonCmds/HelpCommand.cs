using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Client.Networking;
using mcmtestOpenTK.Client.Networking.PacketsOut;

namespace mcmtestOpenTK.Client.CommandHandlers.CommonCmds
{
    class HelpCommand: AbstractCommand
    {
        public HelpCommand()
        {
            Name = "help";
            Arguments = "[help type]";
            Description = "Shows helpful information.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                entry.Bad("<{color.cmdhelp}>/help [help type]\n" +
                    "<{color.base}>Help Types:\n" +
                    "<{color.base}>    textstyle - view help with the text styling (color, bold, etc.)\n" +
                    "<{color.base}>    characters - view a list of what text characters are currently supported.\n" +
                    "<{color.base}>    commands - lists the commands available\n" +
                    "<{color.base}>    command [command name] - view information on a specific command\n" +
                    "<{color.info}>Press CTRL-C to copy console text. Press CTRL-V to paste into the console.\n" +
                    "<{color.info}>Press the PageUp (PGUP) key to scroll the console text up, or the PageDown (PGDN) key to scroll down.");
            }
            else
            {
                string input = entry.GetArgument(0).ToLower();
                switch (input)
                {
                    case "textstyle":
                    case "text":
                    case "style":
                        entry.Output.Good("^r^7Text Colors: ^0^h^1^^n1 ^!^^n! ^2^^n2 ^@^^n@ ^3^^n3 ^#^^n# ^4^^n4 ^$^^n$ ^5^^n5 ^%^^n% ^6^^n6 ^-^^n- ^7^^n7 ^&^^n& ^8^^n8 ^*^^** ^9^^n9 ^(^^n( ^&^h^0^^n0^h ^)^^n) ^a^^na ^A^^nA\n" +
                            "^r^7Text styles: ^b^^nb is bold,^r ^i^^ni is italic,^r ^u^^nu is underline,^r ^s^^ns is strike-through,^r ^O^^nO is overline,^r ^7^h^0^^nh is highlight,^r^7 ^j^^nj is jello (AKA jiggle),^r " +
                            "^2^e^0^^ne is emphasis,^r^7 ^t^^nt is transparent,^r ^T^^nT is more transparent,^r ^o^^no is opaque,^r ^R^^nR is random,^r ^p^^np is pseudo-random,^r ^^nk is obfuscated (^kobfu^r),^r " +
                            "^^nS is ^SSuperScript^r, ^^nl is ^lSubScript (AKA Lower-Text)^r, ^h^8^d^^nd is Drop-Shadow,^r^7 ^f^^nf is flip,^r ^^nr is regular text, ^^nq is a ^qquote^q, and ^^nn is nothing (escape-symbol).");
                        break;
                    case "commands":
                        StringBuilder commandlist = new StringBuilder();
                        for (int i = 0; i < ClientCommands.CommandSystem.RegisteredCommandList.Count; i++)
                        {
                            AbstractCommand c = ClientCommands.CommandSystem.RegisteredCommandList[i];
                            if (c.Name.Length != 0 && c.Name[0] != '\0')
                            {
                                commandlist.Append(TextStyle.Color_Commandhelp + "/" + c.Name + TextStyle.Color_Outgood + " - " + c.Description +
                                    (i + 1 < ClientCommands.CommandSystem.RegisteredCommands.Count ? "\n" : ""));
                            }
                        }
                        entry.Output.Good("There are <{color.emphasis}>" + ClientCommands.CommandSystem.RegisteredCommands.Count
                            + "<{color.base}> clientside commands loaded.\n" + TagParser.Escape(commandlist.ToString()));
                        break;
                    case "command":
                        if (entry.Arguments.Count < 2)
                        {
                            entry.Bad("<{color.cmdhelp}>/help command [command name]");
                        }
                        else
                        {
                            string cmd = entry.Arguments[1].ToLower();
                            bool found = false;
                            for (int i = 0; i < ClientCommands.CommandSystem.RegisteredCommandList.Count; i++)
                            {
                                AbstractCommand c = ClientCommands.CommandSystem.RegisteredCommandList[i];
                                if (c.Name == cmd)
                                {
                                    AbstractCommand.ShowUsage(new CommandEntry() { Name = cmd, Command = c,
                                        Output = entry.Output, Queue = entry.Queue, Arguments = new List<string>(), CommandLine = cmd });
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                if (NetworkBase.IsActive)
                                {
                                    NetworkBase.Send(new CommandPacketOut("help\ncommand\n" + cmd));
                                }
                                else
                                {
                                    entry.Bad("Unknown command '<{color.emphasis}>" + TagParser.Escape(cmd) + "<{color.base}>'.");
                                }
                            }
                        }
                        break;
                    case "characters":
                        entry.Output.Good("The following characters are recognized by the system: <{color.standout}>" + TagParser.Escape(GLFont.textfile));
                        break;
                    default:
                        if (NetworkBase.IsActive)
                        {
                            StringBuilder argstr = new StringBuilder();
                            argstr.Append("help\n").Append(input);
                            for (int i = 1; i < entry.Arguments.Count; i++)
                            {
                                argstr.Append("\n" + entry.GetArgument(i));
                            }
                            NetworkBase.Send(new CommandPacketOut(argstr.ToString()));
                        }
                        else
                        {
                            entry.Bad("Invalid help type! Type '<{color.emphasis}>/help<{color.base}>' to see a list of available help types.");
                        }
                        break;
                }
            }
        }
    }
}

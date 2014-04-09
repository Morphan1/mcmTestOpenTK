using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.UIHandlers;
using mcmtestOpenTK.Shared.CommandSystem;

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
                UIConsole.WriteLine(TextStyle.Color_Commandhelp + "/help [help type]\n" +
                    TextStyle.Color_Outgood + "Help Types:\n" +
                    TextStyle.Color_Outgood + "    textstyle - view help with the text styling (color, bold, etc.)\n" +
                    TextStyle.Color_Outgood + "    characters - view a list of what text characters are currently supported.\n" +
                    TextStyle.Color_Outgood + "    commands - lists the commands available\n" +
                    TextStyle.Color_Outgood + "    command [command name] - view information on a specific command\n" +
                    TextStyle.Color_Importantinfo + "Press CTRL-C to copy console text. Press CTRL-V to paste into the console.\n" +
                    TextStyle.Color_Importantinfo + "Press the PageUp (PGUP) key to scroll the console text up, or the PageDown (PGDN) key to scroll down.");
            }
            else
            {
                string input = entry.GetArgument(0).ToLower();
                switch (input)
                {
                    case "textstyle":
                    case "text":
                    case "style":
                        UIConsole.WriteLine("^r^7Text Colors: ^0^h^1^^n1 ^!^^n! ^2^^n2 ^@^^n@ ^3^^n3 ^#^^n# ^4^^n4 ^$^^n$ ^5^^n5 ^%^^n% ^6^^n6 ^-^^n- ^7^^n7 ^&^^n& ^8^^n8 ^*^^** ^9^^n9 ^(^^n( ^&^h^0^^n0^h ^)^^n) ^a^^na ^A^^nA\n" +
                            "^r^7Text styles: ^b^^nb is bold,^r ^i^^ni is italic,^r ^u^^nu is underline,^r ^s^^ns is strike-through,^r ^O^^nO is overline,^r ^7^h^0^^nh is highlight,^r^7 ^j^^nj is jello (AKA jiggle),^r " +
                            "^2^e^0^^ne is emphasis,^r^7 ^t^^nt is transparent,^r ^T^^nT is more transparent,^r ^o^^no is opaque,^r ^R^^nR is random,^r ^p^^np is pseudo-random,^r ^^nk is obfuscated (^kobfu^r),^r " +
                            "^^nS is ^SSuperScript^r, ^^nl is ^lSubScript (AKA Lower-Text)^r, ^h^8^d^^nd is Drop-Shadow,^r^7 ^f^^nf is flip,^r ^^nr is regular text, ^^nq is a ^qquote^q, and ^^nn is nothing (escape-symbol).");
                        break;
                    case "commands":
                        StringBuilder commandlist = new StringBuilder();
                        for (int i = 0; i < ClientCommands.CommandSystem.RegisteredCommands.Count; i++)
                        {
                            AbstractCommand c = ClientCommands.CommandSystem.RegisteredCommands[i];
                            commandlist.Append(TextStyle.Color_Commandhelp + "/" + c.Name + TextStyle.Color_Outgood + " - " + c.Description +
                                (i + 1 < ClientCommands.CommandSystem.RegisteredCommands.Count ? "\n" : ""));
                        }
                        UIConsole.WriteLine(TextStyle.Color_Outgood + "There are " + TextStyle.Color_Separate + ClientCommands.CommandSystem.RegisteredCommands.Count +
                            TextStyle.Color_Outgood + " clientside commands loaded.\n" + commandlist.ToString());
                        break;
                    case "command":
                        if (entry.Arguments.Count < 2)
                        {
                            UIConsole.WriteLine(TextStyle.Color_Commandhelp + "/help command [command name]");
                        }
                        else
                        {
                            string cmd = entry.Arguments[1].ToLower();
                            bool found = false;
                            for (int i = 0; i < ClientCommands.CommandSystem.RegisteredCommands.Count; i++)
                            {
                                AbstractCommand c = ClientCommands.CommandSystem.RegisteredCommands[i];
                                if (c.Name == cmd)
                                {
                                    AbstractCommand.ShowUsage(new CommandEntry(cmd, null, null) { Name = cmd, Command = c, Output = ClientCommands.CommandSystem.Output });
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                UIConsole.WriteLine(TextStyle.Color_Outbad + "Unknown command '" + TextStyle.Color_Separate + cmd +
                                    TextStyle.Color_Outbad + "'.");
                            }
                        }
                        break;
                    case "characters":
                        UIConsole.WriteLine(TextStyle.Color_Outgood + "The following characters are recognized by the system: " +
                            TextStyle.Color_Standout + mcmtestOpenTK.Client.GraphicsHandlers.Text.GLFont.textfile);
                        break;
                    default:
                        UIConsole.WriteLine(TextStyle.Color_Outbad + "Invalid help type! Type '" + TextStyle.Color_Separate + "/help" +
                            TextStyle.Color_Outbad + "' to see a list of available help types.");
                        break;
                }
            }
        }
    }
}

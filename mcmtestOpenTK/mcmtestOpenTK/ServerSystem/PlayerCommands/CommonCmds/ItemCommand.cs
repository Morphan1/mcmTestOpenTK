using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;
using mcmtestOpenTK.ServerSystem.NetworkHandlers.PacketsOut;
using mcmtestOpenTK.ServerSystem.GameHandlers;

namespace mcmtestOpenTK.ServerSystem.PlayerCommands.CommonCmds
{
    public class ItemCommand: PlayerAbstractCommand
    {
        public ItemCommand()
        {
            Name = "item";
            Arguments = "<name>";
            Description = "Sets your held item.";
        }

        public override void Execute(PlayerCommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
            }
            else
            {
                string itemname = entry.Arguments[0];
                Item item = ItemRegistry.GetItemFor(itemname);
                if (item == null)
                {
                    entry.player.SendMessage("Unknown item.");
                }
                else
                {
                    entry.player.Send(new GiveItemPacketOut(item));
                    entry.player.SendMessage("Item set.");
                }
            }
        }
    }
}

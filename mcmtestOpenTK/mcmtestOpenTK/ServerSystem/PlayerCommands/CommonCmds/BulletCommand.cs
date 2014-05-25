using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.ServerSystem.GameHandlers.Entities;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.PlayerCommands.CommonCmds
{
    public class BulletCommand: PlayerAbstractCommand
    {
        public BulletCommand()
        {
            Name = "bullet";
            Arguments = "";
            Description = "Fires a bullet from the player.";
        }

        public override void Execute(PlayerCommandEntry entry)
        {
            entry.player.SendMessage("Firing...");
            Location forward = Utilities.ForwardVector(entry.player.Direction.X * Utilities.PI180,
                entry.player.Direction.Y * Utilities.PI180);
            Server.MainWorld.Spawn(new Bullet() { Position = entry.player.Position + new Location(0, 0, 6) + forward * 2,
                Direction = entry.player.Direction,
                Velocity = forward * 10});
        }
    }
}

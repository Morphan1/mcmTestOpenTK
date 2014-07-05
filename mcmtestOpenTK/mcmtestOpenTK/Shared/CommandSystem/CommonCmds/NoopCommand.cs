using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Shared.CommandSystem.CommonCmds
{
    class NoopCommand: AbstractCommand
    {
        public NoopCommand()
        {
            Name = "noop";
            Arguments = "";
            Description = "Does nothing.";
            IsDebug = true;
        }

        public override void Execute(CommandEntry entry)
        {
        }
    }
}

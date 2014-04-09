using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.CommandHandlers.CommonCmds;
using mcmtestOpenTK.Client.CommandHandlers.GraphicsCmds;
using mcmtestOpenTK.Client.CommandHandlers.NetworkCmds;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.CommandHandlers
{
    class ClientCommands
    {
        /// <summary>
        /// The Commands object that all commands actually go to.
        /// </summary>
        public static Commands CommandSystem;

        /// <summary>
        /// Prepares the command system, registering all base commands.
        /// </summary>
        public static void Init()
        {
            CommandSystem = new Commands();
            CommandSystem.Output = new ClientOutputter();
            CommandSystem.Init();

            // Common
            CommandSystem.RegisterCommand(new CvarinfoCommand());
            CommandSystem.RegisterCommand(new EchoCommand());
            CommandSystem.RegisterCommand(new HelpCommand());
            CommandSystem.RegisterCommand(new HideconsoleCommand());
            CommandSystem.RegisterCommand(new QuitCommand());
            CommandSystem.RegisterCommand(new SetCommand());
            CommandSystem.RegisterCommand(new ShowconsoleCommand());

            // Graphics
            CommandSystem.RegisterCommand(new LoadshaderCommand());
            CommandSystem.RegisterCommand(new LoadtextureCommand());
            CommandSystem.RegisterCommand(new ReloadCommand());
            CommandSystem.RegisterCommand(new RemapshaderCommand());
            CommandSystem.RegisterCommand(new RemaptextureCommand());
            CommandSystem.RegisterCommand(new ReplacefontCommand());
            CommandSystem.RegisterCommand(new SavetextureCommand());
            CommandSystem.RegisterCommand(new ShaderlistCommand());
            CommandSystem.RegisterCommand(new TexturelistCommand());

            // Network
            CommandSystem.RegisterCommand(new LoginCommand());
            CommandSystem.RegisterCommand(new TimeCommand());
        }

        /// <summary>
        /// Advances any running command queues.
        /// </summary>
        public static void Tick()
        {
            CommandSystem.Tick(MainGame.DeltaF);
        }

        /// <summary>
        /// Executes an arbitrary list of command inputs (separated by newlines, semicolons, ...)
        /// </summary>
        /// <param name="commands">The command string to parse</param>
        public static void ExecuteCommands(string commands)
        {
            CommandSystem.ExecuteCommands(commands);
        }
    }
}

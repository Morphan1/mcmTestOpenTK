using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;
using mcmtestOpenTK.Client.CommandHandlers.CommonCmds;
using mcmtestOpenTK.Client.CommandHandlers.GraphicsCmds;
using mcmtestOpenTK.Client.CommandHandlers.NetworkCmds;
using mcmtestOpenTK.Client.CommandHandlers.AudioCmds;
using mcmtestOpenTK.Client.GlobalHandler;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.Client.CommandHandlers.TagHandlers.Common;

namespace mcmtestOpenTK.Client.CommandHandlers
{
    class ClientCommands
    {
        /// <summary>
        /// The Commands object that all commands actually go to.
        /// </summary>
        public static Commands CommandSystem;

        /// <summary>
        /// The output system.
        /// </summary>
        public static Outputter Output;

        /// <summary>
        /// Prepares the command system, registering all base commands.
        /// </summary>
        public static void Init(Outputter _output)
        {
            CommandSystem = new Commands();
            Output = _output;
            CommandSystem.Output = Output;
            CommandSystem.Init();

            // Common Commands
            CommandSystem.RegisterCommand(new HelpCommand());
            CommandSystem.RegisterCommand(new HideconsoleCommand());
            CommandSystem.RegisterCommand(new QuitCommand());
            CommandSystem.RegisterCommand(new ShowconsoleCommand());

            // Network Commands
            CommandSystem.RegisterCommand(new ConnectCommand());
            CommandSystem.RegisterCommand(new DisconnectCommand());
            CommandSystem.RegisterCommand(new LoginCommand());
            CommandSystem.RegisterCommand(new TimeCommand());

            // Audio Commands
            CommandSystem.RegisterCommand(new PlaysoundCommand());
            CommandSystem.RegisterCommand(new RemapsoundCommand());
            CommandSystem.RegisterCommand(new SoundlistCommand());

            // Graphics Commands
            CommandSystem.RegisterCommand(new LoadshaderCommand());
            CommandSystem.RegisterCommand(new LoadtextureCommand());
            CommandSystem.RegisterCommand(new ReloadCommand());
            CommandSystem.RegisterCommand(new RemapshaderCommand());
            CommandSystem.RegisterCommand(new RemaptextureCommand());
            CommandSystem.RegisterCommand(new ReplacefontCommand());
            CommandSystem.RegisterCommand(new SavetextureCommand());
            CommandSystem.RegisterCommand(new ShaderlistCommand());
            CommandSystem.RegisterCommand(new TexturelistCommand());

            // Tags
            CommandSystem.TagSystem.Register(new RendererTags());
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

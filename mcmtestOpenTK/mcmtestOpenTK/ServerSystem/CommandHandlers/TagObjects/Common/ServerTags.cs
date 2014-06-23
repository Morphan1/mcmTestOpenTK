using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.ServerSystem.GlobalHandlers;

namespace mcmtestOpenTK.ServerSystem.CommandHandlers.TagObjects.Common
{
    public class ServerTags : TemplateTags
    {
        // <--[tag]
        // @Base server
        // @Group Global Information
        // @Mode Server
        // @ReturnType ServerTag
        // @Returns a generic server class full of specific helpful global information tags,
        // such as <@link tag ServerTag.fps><{server.fps}><@/link>.
        // -->
        public ServerTags()
        {
            Name = "server";
        }

        public override string Handle(TagData data)
        {
            data.Shrink();
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name ServerTag.fps
                // @Group Variables
                // @Mode Server
                // @ReturnType TextTag
                // @Returns the current server FPS (frames per second).
                // -->
                case "fps":
                    return new TextTag(Server.FPS.ToString()).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data.Shrink());
            }
        }
    }
}

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
        public ServerTags()
        {
            Name = "server";
        }

        public override string Handle(TagData data)
        {
            data.Shrink();
            if (data.Input.Count == 0)
            {
                return "{TAG_ERROR:EMPTY}";
            }
            switch (data.Input[0])
            {
                case "fps":
                    return new TextTag(Server.FPS.ToString()).Handle(data.Shrink());
                default:
                    return "{TAG_ERROR:UNKNOWN_SUBTAG}";
            }
        }
    }
}

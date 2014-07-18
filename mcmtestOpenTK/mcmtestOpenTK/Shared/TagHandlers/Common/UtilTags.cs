﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared.Collision;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    public class UtilTags: TemplateTags
    {
        // <--[tag]
        // @Base util
        // @Group Utilities
        // @ReturnType UtilTag
        // @Returns a generic utility class full of specific helpful utility tags,
        // such as <@link tag UtilTag.random_decimal><{util.random_decimal}><@/link>.
        // -->
        public UtilTags()
        {
            Name = "util";
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
                // @Name UtilTag.random_decimal
                // @Group Utilities
                // @ReturnType TextTag
                // @Returns a random decimal number between zero and one.
                // -->
                case "random_decimal":
                    return new TextTag(Utilities.random.NextDouble()).Handle(data.Shrink());
                // <--[tag]
                // @Name UtilTag.current_time
                // @Group Utilities
                // @ReturnType TimeTag
                // @Returns the current system time (UTC).
                // -->
                case "current_time":
                    return new TimeTag(DateTime.UtcNow).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }
    }
}

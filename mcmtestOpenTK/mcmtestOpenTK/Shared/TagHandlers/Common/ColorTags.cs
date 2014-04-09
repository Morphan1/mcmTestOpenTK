using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    public class ColorTags: TemplateTags
    {
        public ColorTags()
        {
            Name = "color";
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
                case "emphasis":
                    return new TextTag("^r^5").Handle(data.Shrink());
                case "base":
                    return new TextTag(data.BaseColor).Handle(data.Shrink());
                default:
                    return "{TAG_ERROR:UNKNOWN_COLOR}";
            }
        }
    }
}

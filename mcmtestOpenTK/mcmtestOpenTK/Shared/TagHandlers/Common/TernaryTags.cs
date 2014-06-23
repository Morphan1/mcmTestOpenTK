using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    class TernaryTags : TemplateTags
    {
        public TernaryTags()
        {
            Name = "ternary";
        }

        public override string Handle(TagData data)
        {
            bool basevalue = data.GetModifier(0).ToLower() == "true";
            data.Shrink();
            if (data.Input.Count == 0)
            {
                return "&null";
            }
            if (data.Input[0] != "pass")
            {
                return new TextTag("&null").Handle(data);
            }
            string result = "";
            if (basevalue)
            {
                result = data.GetModifier(0);
            }
            data.Shrink();
            if (data.Input.Count == 0)
            {
                return "&null";
            }
            if (data.Input[0] != "fail")
            {
                return new TextTag("&null").Handle(data);
            }
            if (!basevalue)
            {
                result = data.GetModifier(0);
            }
            return new TextTag(result).Handle(data.Shrink());
        }
    }
}

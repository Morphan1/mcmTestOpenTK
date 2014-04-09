using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    class TextTags : TemplateTags
    {
        public TextTags()
        {
            Name = "text";
        }

        public override string Handle(TagData data)
        {
            string modif = data.GetModifier(0);
            return new TextTag(modif).Handle(data.Shrink());
        }
    }
}

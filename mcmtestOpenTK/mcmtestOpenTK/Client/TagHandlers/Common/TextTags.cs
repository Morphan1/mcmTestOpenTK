using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.TagHandlers.Objects;

namespace mcmtestOpenTK.Client.TagHandlers.Common
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

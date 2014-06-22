using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    class ListTags : TemplateTags
    {
        public ListTags()
        {
            Name = "list";
        }

        public override string Handle(TagData data)
        {
            string modif = data.GetModifier(0);
            return new ListTag(modif).Handle(data.Shrink());
        }
    }
}

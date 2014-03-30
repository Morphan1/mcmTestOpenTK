using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.TagHandlers.Objects;

namespace mcmtestOpenTK.Client.TagHandlers.Common
{
    class VarTags: TemplateTags
    {
        public VarTags()
        {
            Name = "var";
        }

        public override string Handle(TagData data)
        {
            string modif = data.Modifiers[0].ToLower();
            if (String.IsNullOrEmpty(modif))
            {
                return "{TAG_ERROR:NEED_MODIFIER}";
            }
            for (int i = 0; i < data.Variable_Names.Count; i++)
            {
                if (data.Variable_Names[i] == modif)
                {
                    return new TextTag(data.Variables[i]).Handle(data.Shrink());
                }
            }
            return "{UNKNOWN_VARIABLE:" + modif + "}";
        }
    }
}

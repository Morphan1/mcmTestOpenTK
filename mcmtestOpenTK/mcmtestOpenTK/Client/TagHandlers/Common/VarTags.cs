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
            string modif = data.GetModifier(0).ToLower();
            if (String.IsNullOrEmpty(modif))
            {
                return "{TAG_ERROR:NEED_MODIFIER}";
            }
            if (data.Variables != null)
            {
                for (int i = 0; i < data.Variables.Count; i++)
                {
                    if (data.Variables[i].Name == modif)
                    {
                        return new TextTag(data.Variables[i].Value).Handle(data.Shrink());
                    }
                }
            }
            return "{UNKNOWN_VARIABLE:" + modif + "}";
        }
    }
}

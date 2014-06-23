using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    class CVarTags: TemplateTags
    {
        public CVarTags()
        {
            Name = "cvar";
        }

        public override string Handle(TagData data)
        {
            string modif = data.GetModifier(0).ToLower();
            CVar cvar = data.TagSystem.CommandSystem.Output.CVarSys.Get(modif);
            if (cvar != null)
            {
                return new TextTag(cvar.Value).Handle(data.Shrink());
            }
            return new TextTag("").Handle(data.Shrink());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    class CVarTags: TemplateTags
    {
        // <--[tag]
        // @Base cvar[<TextTag>]
        // @Group Variables
        // @ReturnType VariableTag
        // @Returns the specified global control variable.
        // <@link explanation cvars>What are CVars?<@/link>
        // -->
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
                data.Shrink();
                // Meta in VarTags.cs
                if (data.Input.Count > 0 && data.Input[0] == "exists")
                {
                    return new TextTag(true).Handle(data.Shrink());
                }
                else
                {
                    return new TextTag(cvar.Value).Handle(data);
                }
            }
            if (data.Input.Count > 0 && data.Input[0] == "exists")
            {
                return new TextTag(false).Handle(data.Shrink());
            }
            else
            {
                return new TextTag("").Handle(data);
            }
        }
    }
}

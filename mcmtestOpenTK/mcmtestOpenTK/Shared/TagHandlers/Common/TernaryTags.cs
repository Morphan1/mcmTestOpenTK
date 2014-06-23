﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    class TernaryTags : TemplateTags
    {
        // <--[tag]
        // @Base ternary[<TextTag>]
        // @Group Text Comparison
        // @ReturnType TernaryPassTag
        // @Returns the specified pass or fail value.
        // The full tag is formatted as <{ternary[<TextTag>].pass[<TextTag>].fail[<TextTag>]}>
        // Using <@link tag TernaryPassTag.pass[<TextTag>]>Pass<@/link> and <@link tag TernaryFailTag.fail[<TextTag>]>fail<@/link> sub-tags.
        // If the value in ternary[...] is "true", then the contents of .pass[...] will be returned.
        // Otherwise, the contents of .fail[...] will be returned.
        // -->
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
            // <--[tag]
            // @Name TernaryPassTag.pass[<TextTag>]
            // @Group Text Comparison
            // @ReturnType TernaryFailTag
            // @Returns a step in the ternary pass/fail tag.
            // Used as a part of the <@link tag Ternary[<TextTag>]>Ternary<@/link> tag.
            // -->
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
            // <--[tag]
            // @Name TernaryFailTag.fail[<TextTag>]
            // @Group Text Comparison
            // @ReturnType TextTag
            // @Returns a step in the ternary pass/fail tag.
            // Used as a part of the <@link tag Ternary[<TextTag>]>Ternary<@/link> tag.
            // -->
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

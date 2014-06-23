﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    // <--[explanation]
    // @Name Lists
    // @Description
    // A list is a type of tag that contains multiple <@link explanation text tags>Text Tags<@/link>.
    // TODO: Explain better!
    // -->
    class ListTags : TemplateTags
    {
        // <--[tag]
        // @Base list[<TextTag>]
        // @Group Text Comparison
        // @ReturnType ListTag<TextTag>
        // @Returns the specified text as a list.
        // <@link explanation lists>What are lists?<@/link>
        // -->
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

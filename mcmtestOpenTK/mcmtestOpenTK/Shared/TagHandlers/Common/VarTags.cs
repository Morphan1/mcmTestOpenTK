using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Shared.TagHandlers.Common
{
    // <--[explanation]
    // @Name Queue Variables
    // @Description
    // Any given <@link explanation queue>queue<@/link> can have defined variables.
    // Variables are defined primarily by the <@link command define>define<@/link> command,
    // but can be added by other tags and commands, such as the <@link command repeat>repeat<@/link> command.
    // To use a queue variable in a tag, simply use the tag <@link tag var[<TextTag>]><{var[<TextTag>]}><@/link>.
    // TODO: Explain better!
    // -->
    class VarTags: TemplateTags
    {
        // <--[tag]
        // @Base var[<TextTag>]
        // @Group Variables
        // @ReturnType VariableTag
        // @Returns the specified variable from the queue.
        // <@link explanation Queue Variables>What are queue variables?<@/link>
        // -->
        public VarTags()
        {
            Name = "var";
        }

        public override string Handle(TagData data)
        {
            string modif = data.GetModifier(0).ToLower();
            if (data.Variables != null)
            {
                for (int i = 0; i < data.Variables.Count; i++)
                {
                    if (data.Variables[i].Name == modif)
                    {
                        data.Shrink();
                        if (data.Input.Count > 0 && data.Input[0] == "exists")
                        {
                            return new TextTag(true).Handle(data.Shrink());
                        }
                        else
                        {
                            return new TextTag(data.Variables[i].Value).Handle(data);
                        }
                    }
                }
            }
            data.Shrink();
            // <--[tag]
            // @Name VariableTag.exists
            // @Group Variables
            // @ReturnType TextTag
            // @Returns whether the specified variable exists.
            // Specifically for the tags <@link tag var[<TextTag>]><{var[<TextTag>]}><@/link>
            // and  <@link tag cvar[<TextTag>]><{cvar[<TextTag>]}><@/link>
            // -->
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

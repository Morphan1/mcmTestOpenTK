using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Client.CommandHandlers.TagHandlers.Objects
{
    class ShaderTag : TemplateObject
    {
        public Shader shader;

        public ShaderTag(Shader shad)
        {
            shader = shad;
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name ShaderTag.name
                // @Group Shader Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns the name of the shader.
                // -->
                case "name":
                    return new TextTag(shader.Name).Handle(data.Shrink());
                // <--[tag]
                // @Name ShaderTag.internal_id
                // @Group Shader Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns the internal ID that represents this shader (after remapping).
                // Compare to <@link tag ShaderTag.internal_id>ShaderTag.original_internal_id<@/link>.
                // -->
                case "internal_id":
                    return new TextTag(shader.Internal_Program.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name ShaderTag.original_internal_id
                // @Group Shader Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns the internal ID that represents this shader (before remapping).
                // Compare to <@link tag ShaderTag.internal_id>ShaderTag.internal_id<@/link>.
                // -->
                case "original_internal_id":
                    return new TextTag(shader.Original_Program.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name ShaderTag.is_remapped
                // @Group Shader Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns whether this shader loaded properly (default shader if not).
                // -->
                case "loaded_properly":
                    return new TextTag(shader.LoadedProperly.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name ShaderTag.is_remapped
                // @Group Shader Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns whether this shader is remapped.
                // Use with <@link tag ShaderTag.remapped_to>ShaderTag.remapped_to<@/link>.
                // -->
                case "is_remapped":
                    return new TextTag((shader.RemappedTo != null).ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name ShaderTag.remapped_to
                // @Group Shader Information
                // @Mode Client
                // @ReturnType ShaderTag
                // @Returns what shader this shader is remapped to, if any.
                // Use with <@link tag ShaderTag.is_remapped>ShaderTag.is_remapped<@/link>.
                // -->
                case "remapped_to":
                    if (shader.RemappedTo != null)
                    {
                        return new ShaderTag(shader.RemappedTo).Handle(data.Shrink());
                    }
                    else
                    {
                        return new TextTag("&null").Handle(data.Shrink());
                    }
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return shader.Name;
        }
    }
}

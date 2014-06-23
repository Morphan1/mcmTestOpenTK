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
    class TextureTag: TemplateObject
    {
        public Texture texture;

        public TextureTag(Texture text)
        {
            texture = text;
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
                // @Name TextureTag.name
                // @Group Texture Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns the name of the texture.
                // -->
                case "name":
                    return new TextTag(texture.Name).Handle(data.Shrink());
                // <--[tag]
                // @Name TextureTag.width
                // @Group Texture Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns the width (in pixels) on the texture.
                // -->
                case "width":
                    return new TextTag(texture.Width.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name TextureTag.height
                // @Group Texture Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns the height (in pixels) on the texture.
                // -->
                case "height":
                    return new TextTag(texture.Height.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name TextureTag.internal_id
                // @Group Texture Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns the internal ID that represents this texture (before remapping).
                // Compare to <@link tag TextureTag.internal_id>TextureTag.internal_id<@/link>.
                // -->
                case "internal_id":
                    return new TextTag(texture.Internal_Texture.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name TextureTag.original_internal_id
                // @Group Texture Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns the internal ID that represents this texture (before remapping).
                // Compare to <@link tag TextureTag.internal_id>TextureTag.internal_id<@/link>.
                // -->
                case "original_internal_id":
                    return new TextTag(texture.Original_InternalID.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name TextureTag.loaded_properly
                // @Group Texture Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns whether the texture loaded properly (default white-pixel texture if not).
                // -->
                case "loaded_properly":
                    return new TextTag(texture.LoadedProperly.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name TextureTag.is_remapped
                // @Group Texture Information
                // @Mode Client
                // @ReturnType TextTag
                // @Returns whether this texture is remapped.
                // Use with <@link tag TextureTag.remapped_to>TextureTag.remapped_to<@/link>.
                // -->
                case "is_remapped":
                    return new TextTag((texture.RemappedTo != null).ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name TextureTag.remapped_to
                // @Group Texture Information
                // @Mode Client
                // @ReturnType ShaderTag
                // @Returns what texture this texture is remapped to, if any.
                // Use with <@link tag TextureTag.is_remapped>TextureTag.is_remapped<@/link>.
                // -->
                case "remapped_to":
                    if (texture.RemappedTo != null)
                    {
                        return new TextureTag(texture.RemappedTo).Handle(data.Shrink());
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
            return texture.Name;
        }
    }
}

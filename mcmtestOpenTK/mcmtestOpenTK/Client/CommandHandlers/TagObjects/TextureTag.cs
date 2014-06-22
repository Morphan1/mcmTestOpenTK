using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers;
using mcmtestOpenTK.Shared.TagHandlers.Objects;

namespace mcmtestOpenTK.Client.CommandHandlers.TagObjects
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
                case "name":
                    return new TextTag(texture.Name).Handle(data.Shrink());
                case "width":
                    return new TextTag(texture.Width.ToString()).Handle(data.Shrink());
                case "height":
                    return new TextTag(texture.Height.ToString()).Handle(data.Shrink());
                case "internal_id":
                    return new TextTag(texture.Internal_Texture.ToString()).Handle(data.Shrink());
                case "original_internal_id":
                    return new TextTag(texture.Original_InternalID.ToString()).Handle(data.Shrink());
                case "loaded_properly":
                    return new TextTag(texture.LoadedProperly.ToString()).Handle(data.Shrink());
                case "is_remapped":
                    return new TextTag((texture.RemappedTo != null).ToString()).Handle(data.Shrink());
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

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
                case "name":
                    return new TextTag(shader.Name).Handle(data.Shrink());
                case "internal_id":
                    return new TextTag(shader.Internal_Program.ToString()).Handle(data.Shrink());
                case "original_internal_id":
                    return new TextTag(shader.Original_Program.ToString()).Handle(data.Shrink());
                case "loaded_properly":
                    return new TextTag(shader.LoadedProperly.ToString()).Handle(data.Shrink());
                case "is_remapped":
                    return new TextTag((shader.RemappedTo != null).ToString()).Handle(data.Shrink());
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

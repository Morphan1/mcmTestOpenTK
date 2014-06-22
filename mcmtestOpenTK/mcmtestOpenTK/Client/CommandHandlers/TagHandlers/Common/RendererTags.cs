using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;
using mcmtestOpenTK.Client.CommandHandlers.TagObjects;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.TagHandlers.Common
{
    class RendererTags : TemplateTags
    {
        public RendererTags()
        {
            Name = "renderer";
        }

        public override string Handle(TagData data)
        {
            data.Shrink();
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                case "textures":
                    {
                        List<TemplateObject> textures = new List<TemplateObject>();
                        for (int i = 0; i < Texture.LoadedTextures.Count; i++)
                        {
                            textures.Add(new TextureTag(Texture.LoadedTextures[i]));
                        }
                        return new ListTag(textures).Handle(data.Shrink());
                    }
                case "shaders":
                    {
                        List<TemplateObject> shaders = new List<TemplateObject>();
                        for (int i = 0; i < Shader.LoadedShaders.Count; i++)
                        {
                            shaders.Add(new ShaderTag(Shader.LoadedShaders[i]));
                        }
                        return new ListTag(shaders).Handle(data.Shrink());
                    }
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }
    }
}

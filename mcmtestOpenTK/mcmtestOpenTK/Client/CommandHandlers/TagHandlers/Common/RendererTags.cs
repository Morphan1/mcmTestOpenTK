using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers.Objects;
using mcmtestOpenTK.Client.CommandHandlers.TagHandlers.Objects;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Client.CommandHandlers.TagHandlers.Common
{
    class RendererTags : TemplateTags
    {
        // <--[tag]
        // @Base renderer
        // @Group Global Information
        // @Mode Client
        // @ReturnType RendererTag
        // @Returns a generic renderer class full of specific helpful graphics/rendering-related information tags,
        // such as <@link tag RendererTag.textures><{renderer.textures}><@/link>.
        // -->
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
                // <--[tag]
                // @Name RendererTag.textures
                // @Group Texture Information
                // @Mode Client
                // @ReturnType ListTag<TextureTag>
                // @Returns a list of all textures loaded on the client.
                // -->
                case "textures":
                    {
                        List<TemplateObject> textures = new List<TemplateObject>();
                        for (int i = 0; i < Texture.LoadedTextures.Count; i++)
                        {
                            textures.Add(new TextureTag(Texture.LoadedTextures[i]));
                        }
                        return new ListTag(textures).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name RendererTag.shaders
                // @Group Shader Information
                // @Mode Client
                // @ReturnType ListTag<ShaderTag>
                // @Returns a list of all shaders loaded on the client.
                // -->
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

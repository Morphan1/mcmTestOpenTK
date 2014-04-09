using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Shared.TagHandlers.Common;

namespace mcmtestOpenTK.Shared.TagHandlers
{
    public class TagParser
    {
        /// <summary>
        /// All tag handler objects currently registered.
        /// </summary>
        public List<TemplateTags> Handlers = new List<TemplateTags>();

        /// <summary>
        /// Registers a handler object for later usage by tags.
        /// </summary>
        /// <param name="handler">The handler object to register.</param>
        public void Register(TemplateTags handler)
        {
            Handlers.Add(handler);
        }

        /// <summary>
        /// Prepares the tag system.
        /// </summary>
        public void Init()
        {
            Register(new ColorTags());
            Register(new VarTags());
            Register(new TextTags());
            // TODO: CVars, ...
        }

        /// <summary>
        /// Reads and parses all tags inside a string.
        /// </summary>
        /// <param name="base_color">The base color for tags to use</param>
        /// <param name="vars">Any variables in this tag's context</param>
        /// <param name="input">The tagged string</param>
        /// <returns>The string with tags parsed</returns>
        public string ParseTags(string input, string base_color, List<Variable> vars)
        {
            if (input.IndexOf("<{") < 0)
            {
                return input;
            }
            int len = input.Length;
            int blocks = 0;
            StringBuilder blockbuilder = new StringBuilder();
            StringBuilder final = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                if (i + 1 < len && input[i] == '<' && input[i + 1] == '{')
                {
                    blocks++;
                    if (blocks == 1)
                    {
                        i++;
                        continue;
                    }
                }
                else if (i + 1 < len && input[i] == '}' && input[i + 1] == '>')
                {
                    blocks--;
                    if (blocks == 0)
                    {
                        string value = blockbuilder.ToString().ToLower();
                        List<string> split = split = value.Split(new char[] { '.' }).ToList();
                        TagData data = new TagData(this, split, base_color, vars);
                        bool handled = false;
                        for (int x = 0; x < Handlers.Count; x++)
                        {
                            if (Handlers[x].Name == data.Input[0])
                            {
                                final.Append(Handlers[x].Handle(data));
                                handled = true;
                                break;
                            }
                        }
                        if (!handled)
                        {
                            final.Append("{UNKNOWN_TAG:" + data.Input[0] + "}");
                        }
                        blockbuilder = new StringBuilder();
                        i++;
                        continue;
                    }
                }
                if (blocks > 0)
                {
                    switch (input[i])
                    {
                        case '.':
                            if (blocks > 1)
                            {
                                blockbuilder.Append("&dot");
                            }
                            else
                            {
                                blockbuilder.Append(".");
                            }
                            break;
                        case '&':
                            blockbuilder.Append("&amp");
                            break;
                        default:
                            blockbuilder.Append(input[i]);
                            break;
                    }
                }
                else
                {
                    final.Append(input[i]);
                }
            }
            return final.ToString();
        }
    }
}

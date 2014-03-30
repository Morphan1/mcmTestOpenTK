using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.TagHandlers.Common;

namespace mcmtestOpenTK.Client.TagHandlers
{
    public class TagParser
    {
        /// <summary>
        /// All tag handler objects currently registered.
        /// </summary>
        public static List<TemplateTags> Handlers = new List<TemplateTags>();

        /// <summary>
        /// Registers a handler object for later usage by tags.
        /// </summary>
        /// <param name="handler">The handler object to register.</param>
        public static void Register(TemplateTags handler)
        {
            Handlers.Add(handler);
        }

        /// <summary>
        /// Prepares the tag system.
        /// </summary>
        public static void Init()
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
        /// <param name="var_names">The names of any variables in this tag's context</param>
        /// <param name="vars">The value of any variables in this tag's context</param>
        /// <param name="input">The tagged string</param>
        /// <returns>The string with tags parsed</returns>
        public static string ParseTags(string input, string base_color, List<string> var_names, List<string> vars)
        {
            int len = input.Length;
            int blocks = 0;
            StringBuilder blockbuilder = new StringBuilder();
            StringBuilder final = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                if (i + 1 < len && input[i] == '<' && input[i + 1] == '{')
                {
                    blocks++;
                    i++;
                    if (blocks == 1)
                    {
                        continue;
                    }
                }
                else if (i + 1 < len && input[i] == '}' && input[i + 1] == '>')
                {
                    blocks--;
                    i++;
                    if (blocks == 0)
                    {
                        string value = blockbuilder.ToString().ToLower();
                        List<string> split = split = value.Split(new char[] { '.' }).ToList();
                        TagData data = new TagData(split, base_color, var_names, vars);
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

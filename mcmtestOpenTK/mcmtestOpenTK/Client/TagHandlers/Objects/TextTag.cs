using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.TagHandlers.Objects
{
    public class TextTag: TemplateObject
    {
        /// <summary>
        /// The text this TextTag represents.
        /// </summary>
        string Text = null;

        public TextTag(string _text)
        {
            Text = _text;
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return Text;
            }
            switch (data.Input[0])
            {
                case "to_upper":
                    return new TextTag(Text.ToUpper()).Handle(data.Shrink());
                case "to_lower":
                    return new TextTag(Text.ToLower()).Handle(data.Shrink());
                default:
                    return "{UNKNOWN_TAG_BIT:" + data.Input[0] + "}";
            }
        }
    }
}

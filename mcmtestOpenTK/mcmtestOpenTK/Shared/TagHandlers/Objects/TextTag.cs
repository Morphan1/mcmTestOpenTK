using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared.TagHandlers.Objects
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

        public TextTag(bool _text)
        {
            Text = _text ? "true" : "false";
        }

        public TextTag(int _text)
        {
            Text = _text.ToString();
        }

        public TextTag(long _text)
        {
            Text = _text.ToString();
        }

        public TextTag(float _text)
        {
            Text = _text.ToString();
        }

        public TextTag(double _text)
        {
            Text = _text.ToString();
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                case "to_upper":
                    return new TextTag(Text.ToUpper()).Handle(data.Shrink());
                case "to_lower":
                    return new TextTag(Text.ToLower()).Handle(data.Shrink());
                case "length":
                    return new TextTag(Text.Length).Handle(data.Shrink());
                case "equals":
                    return new TextTag(Text == data.GetModifier(0)).Handle(data.Shrink());
                case "does_not_equal":
                    return new TextTag(Text != data.GetModifier(0)).Handle(data.Shrink());
                case "and":
                    return new TextTag(Text.ToLower() == "true" && data.GetModifier(0).ToLower() == "true").Handle(data.Shrink());
                case "or":
                    return new TextTag(Text.ToLower() == "true" || data.GetModifier(0).ToLower() == "true").Handle(data.Shrink());
                case "xor":
                    return new TextTag((Text.ToLower() == "true") != (data.GetModifier(0).ToLower() == "true")).Handle(data.Shrink());
                case "equals_ignore_case":
                    return new TextTag(Text.ToLower() == data.GetModifier(0).ToLower()).Handle(data.Shrink());
                case "is_greater_than":
                    return new TextTag(Utilities.StringToDouble(Text) >=
                        Utilities.StringToDouble(data.GetModifier(0))).Handle(data.Shrink());
                case "is_greater_than_or_equal_to":
                    return new TextTag(Utilities.StringToDouble(Text) >=
                        Utilities.StringToDouble(data.GetModifier(0))).Handle(data.Shrink());
                case "is_less_than":
                    return new TextTag(Utilities.StringToDouble(Text) <
                        Utilities.StringToDouble(data.GetModifier(0))).Handle(data.Shrink());
                case "is_less_than_or_equal_to":
                    return new TextTag(Utilities.StringToDouble(Text) <=
                        Utilities.StringToDouble(data.GetModifier(0))).Handle(data.Shrink());
                case "round":
                    return new TextTag(Math.Round(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "absolute_value":
                    return new TextTag(Math.Abs(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "cosine":
                    return new TextTag(Math.Cos(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "sine":
                    return new TextTag(Math.Sin(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "arccosine":
                    return new TextTag(Math.Acos(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "arcsine":
                    return new TextTag(Math.Asin(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "arctangent":
                    return new TextTag(Math.Atan(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "tangent":
                    return new TextTag(Math.Tan(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "atan2":
                    return new TextTag(Math.Atan2(Utilities.StringToDouble(Text),
                        Utilities.StringToDouble(data.GetModifier(0)))).Handle(data.Shrink());
                case "round_up":
                    return new TextTag(Math.Ceiling(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "round_down":
                    return new TextTag(Math.Floor(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "log10":
                    return new TextTag(Math.Log10(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "log":
                    return new TextTag(Math.Log(Utilities.StringToDouble(Text),
                        Utilities.StringToDouble(data.GetModifier(0)))).Handle(data.Shrink());
                case "maximum":
                    return new TextTag(Math.Max(Utilities.StringToDouble(Text),
                        Utilities.StringToDouble(data.GetModifier(0)))).Handle(data.Shrink());
                case "minimum":
                    return new TextTag(Math.Min(Utilities.StringToDouble(Text),
                        Utilities.StringToDouble(data.GetModifier(0)))).Handle(data.Shrink());
                case "to_the_power_of":
                    return new TextTag(Math.Pow(Utilities.StringToDouble(Text),
                        Utilities.StringToDouble(data.GetModifier(0)))).Handle(data.Shrink());
                case "sign":
                    return new TextTag(Math.Sign(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "hyperbolic_sine":
                    return new TextTag(Math.Sinh(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "hyperbolic_cosine":
                    return new TextTag(Math.Cosh(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "hyperbolic_tangent":
                    return new TextTag(Math.Tanh(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "square_root":
                    return new TextTag(Math.Sqrt(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                case "truncate":
                    return new TextTag(Math.Truncate(Utilities.StringToDouble(Text))).Handle(data.Shrink());
                default:
                    return "&{UNKNOWN_TAG_BIT:" + data.Input[0] + "}";
            }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared.TagHandlers.Objects
{
    public class ListTag: TemplateObject
    {
        /// <summary>
        /// The list this ListTag represents.
        /// </summary>
        public List<TemplateObject> ListEntries;

        public ListTag()
        {
            ListEntries = new List<TemplateObject>();
        }

        public ListTag(List<TemplateObject> entries)
        {
            ListEntries = new List<TemplateObject>(entries);
        }

        public ListTag(string list)
        {
            string[] baselist = list.Split('|');
            ListEntries = new List<TemplateObject>();
            for (int i = 0; i < baselist.Length; i++)
            {
                ListEntries.Add(new TextTag(baselist[i].Replace("&pipe", "|").Replace("&amp", "&")));
            }
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
                // @Name ListTag.size
                // @Group List Attributes
                // @ReturnType TextTag
                // @Returns the number of entries in the list.
                // -->
                case "size":
                    return new TextTag(ListEntries.Count.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name ListTag.comma_separated
                // @Group List Attributes
                // @ReturnType TextTag
                // @Returns the list in a user-friendly comma-separated format.
                // EG, "one|two|three" becomes "one, two, three".
                // -->
                case "comma_separated":
                    return new TextTag(ToCSString()).Handle(data.Shrink());
                // <--[tag]
                // @Name ListTag.formatted
                // @Group List Attributes
                // @ReturnType TextTag
                // @Returns the list in a user-friendly format.
                // EG, "one|two|three" becomes "one, two, and three",
                // "one|two" becomes "one and two".
                // -->
                case "formatted":
                    return new TextTag(Formatted()).Handle(data.Shrink());
                // <--[tag]
                // @Name ListTag.reversed
                // @Group List Attributes
                // @ReturnType ListTag
                // @Returns the list entirely backwards.
                // EG, "one|two|three" becomes "three|two|one".
                // -->
                case "reversed":
                    {
                        ListTag newlist = new ListTag(ListEntries);
                        newlist.ListEntries.Reverse();
                        return newlist.Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name ListTag.first
                // @Group List Attributes
                // @ReturnType Dynamic
                // @Returns the first entry in the list.
                // EG, "one|two|three" gets "one".
                // -->
                case "first":
                    if (ListEntries.Count == 0)
                    {
                        return new TextTag("&null").Handle(data.Shrink());
                    }
                    return ListEntries[0].Handle(data.Shrink());
                // <--[tag]
                // @Name ListTag.last
                // @Group List Attributes
                // @ReturnType Dynamic
                // @Returns the last entry in the list.
                // EG, "one|two|three" gets "three".
                // -->
                case "last":
                    if (ListEntries.Count == 0)
                    {
                        return new TextTag("&null").Handle(data.Shrink());
                    }
                    return ListEntries[ListEntries.Count - 1].Handle(data.Shrink());
                // <--[tag]
                // @Name ListTag.get[<TextTag>]
                // @Group List Attributes
                // @ReturnType Dynamic
                // @Returns the specified entry in the list.
                // Note that indices are zero-based.
                // EG, "one|two|three" .get[1] gets "two".
                // -->
                case "get":
                    {
                        int number = Utilities.StringToInt(data.GetModifier(0));
                        if (ListEntries.Count == 0)
                        {
                            return new TextTag("&null").Handle(data.Shrink());
                        }
                        if (number < 0)
                        {
                            number = 0;
                        }
                        if (number >= ListEntries.Count)
                        {
                            number = ListEntries.Count - 1;
                        }
                        return ListEntries[number].Handle(data.Shrink());
                    }
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ListEntries.Count; i++)
            {
                sb.Append(ListEntries[i].ToString().Replace("&", "&amp").Replace("|", "&pipe"));
                if (i + 1 < ListEntries.Count)
                {
                    sb.Append("|");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Renders the list as a comma-separated string (no escaping).
        /// </summary>
        public string ToCSString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ListEntries.Count; i++)
            {
                sb.Append(ListEntries[i].ToString());
                if (i + 1 < ListEntries.Count)
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Renders the list as a comma-separated string with 'and' and similar constructs.
        /// </summary>
        public string Formatted()
        {
            if (ListEntries.Count == 2)
            {
                return (ListEntries[0] + " and " + ListEntries[1]);
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ListEntries.Count; i++)
            {
                sb.Append(ListEntries[i].ToString());
                if (i + 2 == ListEntries.Count)
                {
                    sb.Append(", and ");
                }
                else if (i + 1 < ListEntries.Count)
                {
                    sb.Append(", ");
                }
            }
            return sb.ToString();
        }
    }
}

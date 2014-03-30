using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.TagHandlers
{
    public class TagData
    {
        /// <summary>
        /// The tags current simplified input data.
        /// </summary>
        public List<string> Input = null;

        /// <summary>
        /// All 'modifier' data (EG, input[modifier].input[modifer]).
        /// </summary>
        public List<string> Modifiers = null;

        /// <summary>
        /// The names of all the variables waiting in this tag's context.
        /// </summary>
        public List<string> Variable_Names = null;

        /// <summary>
        /// All variables waiting in this tag's context.
        /// </summary>
        public List<string> Variables = null;

        /// <summary>
        /// The 'base color' set by the tag requesting code.
        /// </summary>
        public string BaseColor = null;

        public TagData(List<string> _input, string _basecolor, List<string> _names, List<string> _vars)
        {
            Input = _input;
            BaseColor = _basecolor;
            Variable_Names = _names;
            Variables = _vars;
            Modifiers = new List<string>();
            for (int x = 0; x < Input.Count; x++)
            {
                Input[x] = Input[x].Replace("&dot", ".").Replace("&amp", "&");
                if (Input[x].Length > 1 && Input[x].Contains('[') && Input[x][Input[x].Length - 1] == ']')
                {
                    int index = Input[x].IndexOf('[');
                    Modifiers.Add(Input[x].Substring(index + 1, Input[x].Length - (index + 2)));
                    Input[x] = Input[x].Substring(0, index);
                }
                else
                {
                    Modifiers.Add("");
                }
            }
        }

        /// <summary>
        /// Shrinks the data amount by one at the start, and returns itself.
        /// </summary>
        /// <returns>This object</returns>
        public TagData Shrink()
        {
            if (Input.Count > 0)
            {
                Input.RemoveAt(0);
            }
            if (Modifiers.Count > 0)
            {
                Modifiers.RemoveAt(0);
            }
            return this;
        }
    }
}

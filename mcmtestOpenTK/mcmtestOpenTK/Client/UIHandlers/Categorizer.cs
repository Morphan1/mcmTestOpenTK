using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Client.UIHandlers
{
    public class Categorizer
    {
        /// <summary>
        /// The name of this category.
        /// </summary>
        public string name = null;

        /// <summary>
        /// A list of the names of all information directly within this category.
        /// </summary>
        public List<string> directly_within_name = new List<string>();

        /// <summary>
        /// A list of the values of all information directly within this category.
        /// </summary>
        public List<string> directly_within_value = new List<string>();

        /// <summary>
        /// A list of all categories within this one.
        /// </summary>
        public List<Categorizer> sub_categories = new List<Categorizer>();

        /// <summary>
        /// The category containing this category.
        /// </summary>
        public Categorizer parent = null;

        /// <summary>
        /// Creates a category object with a specified parent.
        /// </summary>
        /// <param name="_name">The name of the category</param>
        /// <param name="_parent">The parent of this category object, or null if none</param>
        public Categorizer(string _name, Categorizer _parent)
        {
            name = _name;
            parent = _parent;
            if (parent != null)
            {
                parent.sub_categories.Add(this);
            }
        }

        static char[] dotsplit = new char[] { '.' };

        /// <summary>
        /// Gets the object at the specified target, or passes it along to a sub category.
        /// </summary>
        /// <param name="target">What object to get</param>
        /// <returns>The object targeted, or null if none</returns>
        public string Get(string target)
        {
            if (target.Contains('.'))
            {
                string[] datum = target.Split(dotsplit, 2);
                for (int i = 0; i < sub_categories.Count; i++)
                {
                    if (sub_categories[i].name == datum[0])
                    {
                        return sub_categories[i].Get(datum[1]);
                    }
                }
                return null;
            }
            else
            {
                for (int i = 0; i < directly_within_name.Count; i++)
                {
                    if (directly_within_name[i] == target)
                    {
                        return directly_within_value[i];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Set a string directly within the category.
        /// </summary>
        /// <param name="name">The name of the string</param>
        /// <param name="value">The value of the string</param>
        public void Set(string name, string value)
        {
            for (int i = 0; i < directly_within_name.Count; i++)
            {
                if (directly_within_name[i] == name)
                {
                    directly_within_value[i] = value;
                    return;
                }
            }
            directly_within_name.Add(name);
            directly_within_value.Add(value);
        }

        /// <summary>
        /// Creates a child directly within the category, and returns it.
        /// If a category already exists by that name, will return the existing category.
        /// </summary>
        /// <param name="name">The name of the category to create</param>
        /// <returns>The created category</returns>
        public Categorizer CreateChild(string name)
        {
            for (int i = 0; i < sub_categories.Count; i++)
            {
                if (sub_categories[i].name == name)
                {
                    return sub_categories[i];
                }
            }
            Categorizer toret = new Categorizer(name, this);
            sub_categories.Add(toret);
            return toret;
        }
    }
}

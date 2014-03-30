using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.TagHandlers;

namespace mcmtestOpenTK.Client.UIHandlers
{
    public class LanguageHandler
    {
        public static Categorizer Base = null;

        /// <summary>
        /// Prepares the system, pre-loading the 'en-us' data, which other languages can then overwrite.
        /// </summary>
        public static void Init()
        {
            Base = new Categorizer("BASE", null);
            LoadLanguage("en-us");
        }

        static char[] colonsplit = new char[] { ':' };

        /// <summary>
        /// Loads a specified language into the text system for immediate use.
        /// </summary>
        /// <param name="language">Which language to use</param>
        public static void LoadLanguage(string language)
        {
            if (language.Contains('.') || language.Contains('/') || language.Contains('^'))
            {
                ErrorHandler.HandleError("Refused attempt to load language '" + TextStyle.Color_Separate + language + TextStyle.Color_Error + "': invalid name!");
                return;
            }
            try
            {
                if (!FileHandler.Exists("info/languages/" + language + ".txt"))
                {
                    ErrorHandler.HandleError("Failed to load language '" + TextStyle.Color_Separate + language + TextStyle.Color_Error + "': pack file doesn't exist!");
                    return;
                }
                string[] fdata = FileHandler.ReadText("info/languages/" + language + ".txt").Split('\n');
                int levels = -1;
                Categorizer curcat = Base;
                for (int i = 0; i < fdata.Length; i++)
                {
                    string datum = fdata[i].Trim();
                    if (datum.StartsWith("#") || datum.Length == 0)
                    {
                        continue;
                    }
                    int clevel = Utilities.CountCharacter(fdata[i], '\t');
                    if (datum.EndsWith(":") && Utilities.CountCharacter(datum, ':') == 1)
                    {
                        if (clevel <= levels + 1)
                        {
                            while (clevel <= levels)
                            {
                                curcat = curcat.parent;
                                levels--;
                            }
                            curcat = curcat.CreateChild(datum.Substring(0, datum.Length - 1));
                            levels++;
                        }
                        else
                        {
                            ErrorHandler.HandleError("Failed to load language '" + TextStyle.Color_Separate + language + TextStyle.Color_Error +
                                "': invalid leveling within pack at line " + TextStyle.Color_Separate + (i + 1).ToString() + TextStyle.Color_Error + "!");
                            return;
                        }
                    }
                    else if (datum.Contains(':'))
                    {
                        string[] name_value = datum.Split(colonsplit, 2);
                        if (clevel <= levels + 1)
                        {
                            while (clevel <= levels)
                            {
                                curcat = curcat.parent;
                                levels--;
                            }
                            curcat.Set(name_value[0], name_value[1].Substring(1));
                        }
                        else
                        {
                            ErrorHandler.HandleError("Failed to load language '" + TextStyle.Color_Separate + language + TextStyle.Color_Error +
                                "': invalid leveling within pack at line " + TextStyle.Color_Separate + (i + 1).ToString() + TextStyle.Color_Error + "!");
                            return;
                        }
                    }
                    else
                    {
                        ErrorHandler.HandleError("Failed to load language '" + TextStyle.Color_Separate + language + TextStyle.Color_Error +
                            "': invalid data within pack at line " + TextStyle.Color_Separate + (i + 1).ToString() + TextStyle.Color_Error + "!");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Language pack loading for '" + TextStyle.Color_Separate + language + TextStyle.Color_Error + "'", ex);
            }
        }

        /// <summary>
        /// Gets the message corresponding to a name, or a backup error message.
        /// </summary>
        /// <param name="name">The name of the message</param>
        /// <param name="base_color">The wrapping color, for messages that modify color</param>
        /// <param name="var_names">The names of any variables to add</param>
        /// <param name="vars">The values of any variables to add</param>
        /// <returns>The message</returns>
        public static string GetMessage(string name, string base_color, List<string> var_names = null, List<string> vars = null)
        {
            string toret = Base.Get(name.ToLower());
            if (toret == null)
            {
                toret = Base.Get("general.nomessage");
                if (toret == null)
                {
                    toret = "(MISSING MESSAGE '<{COLOR_EMPHASIS}><{var[message]}><{COLOR_BASE}>' + MISSING GENERAL)";
                    if (var_names == null)
                    {
                        var_names = new List<string>();
                        vars = new List<string>();
                    }
                    var_names.Add("message");
                    vars.Add(name.ToLower());
                }
            }
            return TagParser.ParseTags(toret, base_color, var_names, vars);
        }
    }
}

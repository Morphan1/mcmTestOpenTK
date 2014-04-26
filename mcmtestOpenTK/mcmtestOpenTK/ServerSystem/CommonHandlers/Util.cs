using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.ServerSystem.CommonHandlers
{
    public class Util
    {
        /// <summary>
        /// Identifies whether a username is reasonable enough to use.
        /// </summary>
        /// <param name="username">The username to try.</param>
        /// <returns></returns>
        public static bool IsAcceptableName(string username)
        {
            if (username.Length > 20)
            {
                return false;
            }
            if (username.Length < 6)
            {
                return false;
            }
            if (!((username[0] >= 'a' && username[0] <= 'z') || (username[0] >= 'A' && username[0] <= 'Z')))
            {
                return false;
            }
            for (int i = 0; i < username.Length; i++)
            {
                if (!((username[i] >= 'a' && username[i] <= 'z') ||
                    (username[i] >= 'A' && username[i] <= 'Z') ||
                    (username[i] >= '0' && username[i] <= '9') || (username[i] == '_')))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.CommandSystem;

namespace mcmtestOpenTK.Shared
{
    public class CVarSystem
    {
        /// <summary>
        /// A list of all existent CVars.
        /// </summary>
        public List<CVar> CVars;

        /// <summary>
        /// The client/server outputter to use.
        /// </summary>
        public Outputter Output;

        public CVarSystem(Outputter _output)
        {
            CVars = new List<CVar>();
            Output = _output;
            Output.CVarSys = this;
        }

        /// <summary>
        /// Registers a new CVar.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The default value</param>
        /// <returns>The registered CVar</returns>
        public CVar Register(string CVar, string value, CVarFlag flags)
        {
            CVar cvar = new CVar(CVar.ToLower(), value, flags, this);
            CVars.Add(cvar);
            return cvar;
        }

        /// <summary>
        /// Sets the value of an existing CVar, or generates a new one.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The value to set it to</param>
        /// <param name="force">Whether to force a server send</param>
        /// <returns>The set CVar</returns>
        public CVar AbsoluteSet(string CVar, string value, bool force = false)
        {
            CVar gotten = Get(CVar);
            if (gotten == null)
            {
                gotten = Register(CVar, value, CVarFlag.UserMade);
            }
            else
            {
                gotten.Set(value, force);
            }
            return gotten;
        }

        /// <summary>
        /// Gets an existing CVar, or generates a new one with a specific default value.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <param name="value">The default value if it doesn't exist</param>
        /// <returns>The found CVar</returns>
        public CVar AbsoluteGet(string CVar, string value)
        {
            CVar gotten = Get(CVar);
            if (gotten == null)
            {
                gotten = Register(CVar, value, CVarFlag.UserMade);
            }
            return gotten;
        }

        /// <summary>
        /// Gets the CVar that matches a specified name.
        /// </summary>
        /// <param name="CVar">The name of the CVar</param>
        /// <returns>The found CVar, or null if none</returns>
        public CVar Get(string CVar)
        {
            string cvlow = CVar.ToLower();
            for (int i = 0; i < CVars.Count; i++)
            {
                if (CVars[i].Name == cvlow)
                {
                    return CVars[i];
                }
            }
            return null;
        }
    }
}

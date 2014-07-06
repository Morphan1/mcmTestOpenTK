using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.TagHandlers;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public class ScriptEvent
    {
        /// <summary>
        /// All scripts that handle this event.
        /// </summary>
        public List<CommandScript> Handlers;

        /// <summary>
        /// The command system in use.
        /// </summary>
        public Commands System;

        public ScriptEvent(Commands _system, string _name)
        {
            Handlers = new List<CommandScript>();
            System = _system;
            Name = _name.ToLower();
        }

        /// <summary>
        /// Calls the event. Returns whether it was cancelled.
        /// </summary>
        /// <param name="Variables">Any variables to add</param>
        /// <returns>Whether to cancel</returns>
        public bool Call(Dictionary<string, string> Variables)
        {
            if (Variables == null)
            {
                Variables = new Dictionary<string, string>();
            }
            bool cancelled = false;
            for (int i = 0; i < Handlers.Count; i++)
            {
                CommandScript script = Handlers[i];
                Variables.Remove("cancelled");
                Variables.Add("cancelled", cancelled ? "true" : "false");
                string determ = System.ExecuteScript(script, Variables);
                if (determ != null && determ.ToLower() == "cancelled")
                {
                    cancelled = true;
                }
                else if (determ != null && determ.ToLower() == "uncancelled")
                {
                    cancelled = false;
                }
                if (i >= Handlers.Count || Handlers[i] != script)
                {
                    i--;
                }
            }
            return cancelled;
        }

        /// <summary>
        /// The name of this event.
        /// </summary>
        public readonly string Name;
    }
}

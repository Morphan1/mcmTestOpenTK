using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared.CommandSystem
{
    public abstract class Outputter
    {
        /// <summary>
        /// Writes a line of text to the console.
        /// </summary>
        /// <param name="text">The line of text</param>
        public abstract void WriteLine(string text);

        /// <summary>
        /// Writes text to the console.
        /// </summary>
        /// <param name="text">The text to write</param>
        public abstract void Write(string text);
    }
}

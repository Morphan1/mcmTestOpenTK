using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.GraphicsHandlers
{
    public abstract class Renderable
    {
        /// <summary>
        /// Override this method will Graphics drawing code.
        /// </summary>
        public abstract void Draw();
    }
}

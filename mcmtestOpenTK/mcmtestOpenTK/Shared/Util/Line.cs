using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mcmtestOpenTK.Shared.Util
{
    public class Line
    {
        public Location Start;
        public Location End;
        public Line(Location _Start, Location _End)
        {
            Start = _Start;
            End = _End;
        }
    }
}

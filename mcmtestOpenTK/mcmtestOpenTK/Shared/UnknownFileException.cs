using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mcmtestOpenTK.Shared
{
    class UnknownFileException: FileNotFoundException
    {
        public UnknownFileException(string filename)
            : base("file not found", filename)
        {
        }
    }
}

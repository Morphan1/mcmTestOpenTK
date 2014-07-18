using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mcmtestOpenTK.Shared.Util
{
    public class DataStream : MemoryStream
    {
        public DataStream(byte[] bytes)
            : base(bytes)
        {
        }

        public DataStream()
            : base()
        {
        }
    }
}

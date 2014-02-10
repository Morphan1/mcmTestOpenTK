using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mcmtestOpenTK.CommonHandlers
{
    class ErrorHandler
    {
        public static void HandleError(Exception ex)
        {
            File.AppendAllText("errors.log", "ERROR at " + DateTime.Now.ToString() + ": " + ex.ToString() + "\n\n\n");
            Console.WriteLine(ex.ToString());
        }
        public static void HandleOutput(string outp)
        {
            File.AppendAllText("output.log", outp + "\n");
        }
    }
}

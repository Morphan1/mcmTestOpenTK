using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mcmtestOpenTK.CommonHandlers
{
    class ErrorHandler
    {
        /// <summary>
        /// Handles and reports an Exception.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        public static void HandleError(Exception ex)
        {
            HandleError(ex.ToString());
        }

        /// <summary>
        /// Reports an error message.
        /// </summary>
        /// <param name="error">The message to report.</param>
        public static void HandleError(string error)
        {
            File.AppendAllText("errors.log", "ERROR at " + DateTime.Now.ToString() + ": " + error + "\n\n\n");
            Console.WriteLine(error);
        }

        // Temporary, for testing.
        public static void HandleOutput(string outp)
        {
            File.AppendAllText("output.log", outp.Replace('\r', ' ') + "\n");
        }
    }
}

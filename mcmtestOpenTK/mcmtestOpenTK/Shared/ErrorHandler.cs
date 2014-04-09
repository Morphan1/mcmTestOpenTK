using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared;
using System.Threading;

namespace mcmtestOpenTK.Shared
{
    class ErrorHandler
    {
        /// <summary>
        /// Handles and reports an exception.
        /// </summary>
        /// <param name="ex">The exception to handle</param>
        public static void HandleError(Exception ex)
        {
            if (ex is ThreadAbortException)
            {
                throw ex;
            }
            HandleError("Internal unidentified error at " + Utilities.DateTimeToString(DateTime.Now) + ": " + ex.ToString());
        }

        /// <summary>
        /// Handles and reports an exception with a specified cause.
        /// </summary>
        /// <param name="cause">The cause of the exception</param>
        /// <param name="ex">The exception to handle</param>
        public static void HandleError(string cause, Exception ex)
        {
            if (ex is ThreadAbortException)
            {
                throw ex;
            }
            HandleError("Error at " + Utilities.DateTimeToString(DateTime.Now) + ": " + cause + ": " + ex.ToString());
        }

        /// <summary>
        /// Reports an error message.
        /// </summary>
        /// <param name="error">The message to report</param>
        public static void HandleError(string error)
        {
            FileHandler.AppendText("errors.log", error + "\n\n\n");
            SysConsole.Output(OutputType.ERROR, error);
        }
    }
}

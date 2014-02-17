using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Client.GraphicsHandlers.Text;
using mcmtestOpenTK.Shared;

namespace mcmtestOpenTK.Client.CommonHandlers
{
    class ErrorHandler
    {
        /// <summary>
        /// Handles and reports an Exception.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        public static void HandleError(Exception ex)
        {
            HandleError("Internal unidentified error at " + Utilities.DateTimeToString(DateTime.Now) + ": " + ex.ToString());
        }

        public static void HandleError(string cause, Exception ex)
        {
            HandleError("Error at " + Utilities.DateTimeToString(DateTime.Now) + ": " + cause + ": " + ex.ToString());
        }

        /// <summary>
        /// Reports an error message.
        /// </summary>
        /// <param name="error">The message to report.</param>
        public static void HandleError(string error)
        {
            FileHandler.AppendText("errors.log", error + "\n\n\n");
            Console.WriteLine(TextStyle.Color_Error + error);
        }

        // Temporary, for testing.
        public static void HandleOutput(string outp)
        {
            FileHandler.AppendText("output.log", outp + "\n");
        }
    }
}

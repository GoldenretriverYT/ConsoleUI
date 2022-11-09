using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib
{
    internal class WarningManager
    {
        public static List<string> LoggedWarnings { get; set; } = new();

        /// <summary>
        /// Logs a warning once. If a warning with the specified id was already logged, it wont do it again.
        /// </summary>
        /// <param name="warningId">Warning ID</param>
        /// <param name="warningMessage">Message of the warning</param>
        public static void ShowWarningOnce(string warningId, string warningMessage)
        {
            if (LoggedWarnings.Contains(warningId)) return;
            LoggedWarnings.Add(warningId);

            Debug.WriteLine("[Warning] " + warningMessage);
        }
    }
}

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

        public static void ShowWarningOnce(string warningId, string warningMessage)
        {
            if (LoggedWarnings.Contains(warningId)) return;
            LoggedWarnings.Add(warningId);

            Debug.WriteLine("[Warning] " + warningMessage);
        }
    }
}

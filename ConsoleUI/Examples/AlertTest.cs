using ConsoleUILib;
using ConsoleUILib.Controls;
using ConsoleUILib.Window;

using ConsoleUILib;
using ConsoleUILib.Controls;
using ConsoleUILib.Window;
using ConsoleUILib.Window.Special;

namespace ConsoleUI
{
    /// <summary>
    ///
    /// </summary>
    internal class AlertTest
    {
        public static void Start()
        {
            Alert alertWindow = new("Confirmation", "Are you sure?", AlertButtons.YES_NO);

            UIManager.AddWindow(alertWindow);
            UIManager.Start();
        }
    }
}
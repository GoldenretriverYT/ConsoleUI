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
            Alert.ShowAlert("WARNING", "This will delete System32!", AlertButtons.OK_CANCEL, (AlertResult res) =>
            {
                if(res == AlertResult.OK)
                {
                    Alert.ShowAlert("lolz", "Not actually dumbo!", AlertButtons.OK, (AlertResult res) => { });
                }else
                {
                    Alert.ShowAlert("Cancelled", "Action cancelled successfully.", AlertButtons.OK, (AlertResult res) => { });
                }
            });

            UIManager.Start();
        }
    }
}
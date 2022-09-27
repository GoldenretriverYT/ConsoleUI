using ConsoleUILib.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Window.Special
{
    public class Alert : CustomWindow
    {
        private Label textLabel;
        private Button buttonOne;
        private Button buttonTwo;
        private AlertButtons buttons;

        public AlertResult? Result { get; set; } = null;

        public Alert(string title, string text, AlertButtons btns) : base((int)((Console.WindowWidth - (Console.WindowWidth * 0.5)) / 2), (Console.WindowHeight - 5) / 2, (int)(Console.WindowWidth * 0.5), 5)
        {
            this.Title = title;
            this.buttons = btns;
            textLabel = new(this, 1, 0, Width - 2, 1);
            textLabel.Text = text;
            textLabel.HorizontalAlign = HAlign.MIDDLE;
            this.AddControl(textLabel);

            buttonOne = new(this, 1, 2, ((Width - 4) / 2), 1);
            buttonOne.Text = (btns is AlertButtons.OK or AlertButtons.OK_CANCEL ? " OK" : " Yes");
            buttonOne.Pressed += (object sender, EventArgs args) =>
            {
                Result = (btns is AlertButtons.OK or AlertButtons.OK_CANCEL) ? AlertResult.OK : AlertResult.YES;
            };

            this.AddControl(buttonOne);

            if (btns == AlertButtons.OK_CANCEL || btns == AlertButtons.YES_NO) {
                buttonTwo = new(this, 2 + ((Width-4) / 2), 2, ((Width - 4) / 2), 1);
                buttonTwo.Text = (btns == AlertButtons.OK_CANCEL ? " Cancel" : " No");
                buttonTwo.Pressed += (object sender, EventArgs args) =>
                {
                    Result = (btns == AlertButtons.OK_CANCEL ? AlertResult.CANCEL : AlertResult.NO);
                };

                this.AddControl(buttonTwo);
            }
        }

        public static void ShowAlert(string title, string text, AlertButtons btns, Action<AlertResult> callback)
        {
            var alert = new Alert(title, text, btns);
            UIManager.AddWindow(alert);

            new Thread(() =>
            {
                while (alert.Result == null)
                {
                    Thread.Sleep(10);
                }

                callback(alert.Result.Value);
                UIManager.RemoveWindow(alert);
            }).Start();
        }
    }

    public enum AlertButtons
    {
        OK,
        OK_CANCEL,
        YES_NO,
    }

    public enum AlertResult
    {
        OK, CANCEL, YES, NO
    }
}

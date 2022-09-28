using ConsoleUILib;
using ConsoleUILib.Controls;
using ConsoleUILib.Window;

using ConsoleUILib;
using ConsoleUILib.Controls;
using ConsoleUILib.Window;

namespace ConsoleUI
{
    /// <summary>
    /// Basic test with multiple windows. Has 2x 2 buttons and 1 text field
    /// </summary>
    internal class TestMultipleWin
    {
        public static void Start()
        {
            CreateWindow(10);
            CreateWindow(30);
            UIManager.Start();
        }

        private static void CreateWindow(int offX)
        {
            CustomWindow amogus = new CustomWindow(offX, 0, 20, 11);
            amogus.Title = "Text Align Test";
            amogus.RenderDone += (object sender, EventArgs e) => {
                amogus.Title = "Render at " + DateTimeOffset.Now.Hour.ToString("00") + ":" + DateTimeOffset.Now.Minute.ToString("00") + ":" + DateTimeOffset.Now.Second.ToString("00");
            };

            Button leftButton = new Button(amogus, 5, 2, 10, 2);
            leftButton.Text = "dfsdfdfdsfsdfdf";
            leftButton.HorizontalAlign = HAlign.LEFT;
            leftButton.PressAnimation = ButtonPressAnimation.POP_OUT;
            leftButton.Pressed += (object sender, EventArgs e) => {
                (sender as Button).Text = "Pressed!";
            };

            amogus.AddControl(leftButton);

            Button centerButton = new Button(amogus, 5, 5, 10, 2);
            centerButton.Text = "center";
            centerButton.HorizontalAlign = HAlign.MIDDLE;
            centerButton.Pressed += (object sender, EventArgs e) => {
                (sender as Button).Text = "Pressed!";
            };

            amogus.AddControl(centerButton);

            TextField field = new TextField(amogus, 5, 8, 10, 2);
            field.Text = "right";
            field.HorizontalAlign = HAlign.LEFT;
            field.Pressed += (object sender, EventArgs e) => {

            };

            amogus.AddControl(field);

            UIManager.AddWindow(amogus);
        }
    }
}
using ConsoleUILib;
using ConsoleUILib.Controls;
using ConsoleUILib.Window;

namespace ConsoleUI
{
    /// <summary>
    /// Full screen text field for testing
    /// </summary>
    internal class TextFieldTest
    {
        public static void Start()
        {
            CustomWindow amogus = new CustomWindow(2, 0, 60, 20);
            amogus.Title = "Text Align Test";
            amogus.RenderDone += (object sender, EventArgs e) => {
                amogus.Title = "Render at " + DateTimeOffset.Now.Hour.ToString("00") + ":" + DateTimeOffset.Now.Minute.ToString("00") + ":" + DateTimeOffset.Now.Second.ToString("00");
            };

            TextField field = new TextField(amogus, 0, 0, 60, 19);
            field.Text = "abc äüö";
            field.HorizontalAlign = HAlign.LEFT;
            field.VerticalAlign = VAlign.TOP;
            field.Pressed += (object sender, EventArgs e) => {

            };

            amogus.AddControl(field);

            UIManager.AddWindow(amogus);
            UIManager.Start();
        }
    }
}


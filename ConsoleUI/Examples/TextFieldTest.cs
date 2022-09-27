﻿using ConsoleUILib;
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
            CustomWindow amogus = new CustomWindow(2, 0, 20, 10);
            amogus.Title = "Text Align Test";
            amogus.RenderDone += (object sender, EventArgs e) => {
                amogus.Title = "Render at " + DateTimeOffset.Now.Hour.ToString("00") + ":" + DateTimeOffset.Now.Minute.ToString("00") + ":" + DateTimeOffset.Now.Second.ToString("00");
            };

            TextField field = new TextField(amogus, 0, 0, 20, 10);
            field.Text = "right";
            field.HorizontalAlign = HAlign.LEFT;
            field.Pressed += (object sender, EventArgs e) => {

            };

            amogus.AddControl(field);

            UIManager.AddWindow(amogus);
            UIManager.Start();
        }
    }
}


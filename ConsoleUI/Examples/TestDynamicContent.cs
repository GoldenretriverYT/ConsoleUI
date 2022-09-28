using ConsoleUILib;
using ConsoleUILib.Controls;
using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    internal class TestDynamicContent
    {
        public static CustomWindow window;
        public static Random rnd = new();
        public static void Start()
        {
            window = new CustomWindow(0, 0, Console.WindowWidth, Console.WindowHeight);
            window.Title = "Dynamic Content Test";

            CreateNewButton();

            UIManager.AddWindow(window);
            UIManager.Start();
        }

        public static void CreateNewButton()
        {
            Button btn = new Button(window, rnd.Next(Console.WindowWidth-1), rnd.Next(Console.WindowHeight-1), 1, 1);
            btn.Text = "Create new";
            btn.HorizontalAlign = HAlign.LEFT;
            btn.PressAnimation = ButtonPressAnimation.SWITCH_COLOR;
            btn.Pressed += (object sender, EventArgs e) => {
                for (int i = 0; i < 100; i++)
                {
                    CreateNewButton();
                }
            };

            window.AddControl(btn);
        }
    }
}

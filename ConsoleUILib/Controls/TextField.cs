using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls {
    public class TextField : SizedControl {
        public int W { get; set; }
        public int H { get; set; }

        public Color FocusedColor { get; set; } = Color.DimGray;
        public Color PressedColor { get; set; } = Color.FromArgb(26, 26, 26);
        public Color RegularColor { get; set; } = Color.SlateGray;
        public Color TextColor { get; set; } = Color.White;
        public string Text { get; set; } = "TextField";

        public HAlign HorizontalAlign = HAlign.LEFT;
        public VAlign VerticalAlign = VAlign.TOP;

        public EventHandler Pressed;

        private bool wasJustPressed = false;

        public TextField(BaseWindow parent, int x, int y, int width, int height) : base(parent) {
            this.X = x;
            this.Y = y;
            this.W = width;
            this.H = height;
        }

        public override void DrawControl() {
            int xOffset = 0;
            int yOffset = 0;

            if(HorizontalAlign == HAlign.MIDDLE) {
                xOffset = W/2 - (Text.Length/2);
            }else if(HorizontalAlign == HAlign.RIGHT) {
                xOffset = W - Text.Length;
            }

            

            ConsoleCanvas.DrawRect(ActualX, ActualY, W, H, IsSelected ? FocusedColor : RegularColor);
            ConsoleCanvas.DrawString(Text, ActualX + xOffset, ActualY + yOffset, W, H, TextColor, IsSelected ? FocusedColor : RegularColor);
        }


        public override void OnPressed() {
            base.OnPressed();

            wasJustPressed = true;
            ThreadPool.QueueUserWorkItem((obj) => {
                Thread.Sleep(300);
                wasJustPressed = false;
            });

            InvokePressed();
        }

        public override void OnKeyDown(ConsoleKeyInfo key)
        {
            base.OnKeyDown(key);


            Console.WriteLine(IsSelected + " " + key.KeyChar);
            if(IsSelected && char.IsAscii(key.KeyChar))
            {
                Text += key.KeyChar;
            }
        }

        private void InvokePressed() {
            EventHandler handler = Pressed;
            handler?.Invoke(this, new());
        }

        public override Size GetSize() {
            return new(W, H);
        }
    }
}

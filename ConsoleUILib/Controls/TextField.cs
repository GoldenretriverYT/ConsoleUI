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
        public int Cursor { get; set; } = 0;

        public HAlign HorizontalAlign = HAlign.LEFT;
        public VAlign VerticalAlign = VAlign.TOP;

        public EventHandler Pressed;

        public override bool EnterShouldPress => false;

        private bool cursorBlink = false;
        private byte blinkFrames = 5;

        public TextField(BaseWindow parent, int x, int y, int width, int height) : base(parent) {
            this.X = x;
            this.Y = y;
            this.W = width;
            this.H = height;
        }

        public override void DrawControl() {
            int xOffset = 0;
            int yOffset = 0;

            blinkFrames--;

            if(blinkFrames == 0)
            {
                cursorBlink = !cursorBlink;
                blinkFrames = 5;
            }

            if(HorizontalAlign == HAlign.MIDDLE) {
                xOffset = W/2 - (Text.Length/2);
            }else if(HorizontalAlign == HAlign.RIGHT) {
                xOffset = W - Text.Length;
            }

            if (Cursor > Text.Length) Cursor = Text.Length;

            ConsoleCanvas.DrawRect(ActualX, ActualY, W, H, IsSelected ? FocusedColor : RegularColor);
            ConsoleCanvas.DrawString(GetBlinkText(), ActualX + xOffset, ActualY + yOffset, W, H, TextColor, IsSelected ? FocusedColor : RegularColor);
        }

        public string GetBlinkText()
        {
            if (cursorBlink && IsSelected)
            {
                int endBasedOffset = (Cursor == Text.Length ? 0 : 1);

                if (Cursor < Text.Length && Text[Cursor] == '\n')
                {
                    return Text.Substring(0, Cursor) + "█\n" + Text.Substring(Cursor + endBasedOffset, Text.Length - (Cursor + endBasedOffset));
                }
                else
                {
                    return Text.Substring(0, Cursor) + "█" + Text.Substring(Cursor + endBasedOffset, Text.Length - (Cursor + endBasedOffset));
                }
            }
            else
            {
                return Text;
            }
        }

        public override void OnPressed() {
            base.OnPressed();
            Cursor = Text.Length;
            InvokePressed();
        }

        public override void OnKeyDown(ConsoleKeyInfo key)
        {
            base.OnKeyDown(key);

            Debug.WriteLine(IsSelected + " " + key.Key);
            if (IsSelected)
            {
                if (key.Key == ConsoleKey.Enter)
                {
                    AddCharAtCursor('\n');
                }
                else if (key.Key == ConsoleKey.Backspace && Text.Length > 0)
                {
                    Debug.WriteLine("Remvoing last char from text field (cur: " + Text + " len: " + Text.Length + ")");
                    Text = Text.Substring(0, Cursor-1) + Text.Substring(Cursor, Text.Length - Cursor);
                    Cursor--;
                }
                else if (key.Key == ConsoleKey.LeftArrow && Text.Length > 0 && Cursor > 0)
                {
                    Cursor--;
                }
                else if (key.Key == ConsoleKey.RightArrow && Cursor < Text.Length)
                {
                    Cursor++;
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    AddCharAtCursor(key.KeyChar);
                }
            }
        }

        public void AddCharAtCursor(char chr)
        {
            Text = Text.Substring(0, Cursor) + chr + Text.Substring(Cursor, Text.Length - Cursor);
            Cursor++;
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

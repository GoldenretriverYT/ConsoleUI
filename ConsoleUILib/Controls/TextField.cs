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
        /// <summary>
        /// Width of element
        /// </summary>
        public int W { get; set; }
        /// <summary>
        /// Height of element
        /// </summary>
        public int H { get; set; }

        /// <summary>
        /// The color of the text field when <see cref="InteractableControl.IsSelected"/> is true
        /// </summary>
        public Color FocusedColor { get; set; } = Color.DimGray;

        /// <summary>
        /// The color of the text field when its clicked
        /// </summary>
        public Color PressedColor { get; set; } = Color.FromArgb(26, 26, 26);

        /// <summary>
        /// The normal background color of the text field
        /// </summary>
        public Color RegularColor { get; set; } = Color.SlateGray;

        /// <summary>
        /// The color of the text on the button
        /// </summary>
        public Color TextColor { get; set; } = Color.White;

        /// <summary>
        /// The content of the text field
        /// </summary>
        public string Text { get; set; } = "TextField";
        
        /// <summary>
        /// The offset of the cursor
        /// </summary>
        public int Cursor { get; set; } = 0;

        /// <summary>
        /// Horizontal align of the text (CURRENTLY COMPLETELY BROKEN, KEEP AT LEFT)
        /// </summary>
        public HAlign HorizontalAlign = HAlign.LEFT;
        /// <summary>
        /// Vertical align of the text (CURRENTLY COMPLETELY BROKEN, KEEP AT TOP)
        /// </summary>
        public VAlign VerticalAlign = VAlign.TOP;
        
        /// <summary>
        /// The current state of how the content would overflow
        /// </summary>
        public OverflowState OverflowState { get; private set; } = OverflowState.NONE;

        /// <summary>
        /// Called when the text field is clicked
        /// </summary>
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
                if(HorizontalAlign != HAlign.LEFT && VerticalAlign != VAlign.TOP)
                {
                    WarningManager.ShowWarningOnce("warning-alignment-" + RandomizedID, "Changing text alignment of text fields is not recommended as its broken.");
                }
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

            OverflowState = ConsoleCanvas.DrawString(GetBlinkText(), ActualX + xOffset, ActualY + yOffset, W, H, TextColor, IsSelected ? FocusedColor : RegularColor);
        }

        /// <summary>
        /// Gets the content including the cursor
        /// </summary>
        /// <returns></returns>
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
                else if (key.Key == ConsoleKey.Backspace && Text.Length > 0 && Cursor > 0)
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

        /// <summary>
        /// Adds a character at the current cursor position
        /// </summary>
        /// <param name="chr">Character to add</param>
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

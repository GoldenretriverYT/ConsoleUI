using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls {
    /// <summary>
    /// A button is a clickable element, that triggers an event when clicked.
    /// </summary>
    public class Button : SizedControl {
        /// <summary>
        /// Width of element
        /// </summary>
        public int W { get; set; }
        /// <summary>
        /// Height of element
        /// </summary>
        public int H { get; set; }

        /// <summary>
        /// The color of the button when <see cref="InteractableControl.IsSelected"/> is true
        /// </summary>
        public Color FocusedColor { get; set; } = Color.DimGray;

        /// <summary>
        /// The color of the button when its clicked (if <see cref="PressAnimation"/> is set to <see cref="ButtonPressAnimation.SWITCH_COLOR"/>
        /// </summary>
        public Color PressedColor { get; set; } = Color.FromArgb(26, 26, 26);

        /// <summary>
        /// The normal background color of the button
        /// </summary>
        public Color RegularColor { get; set; } = Color.SlateGray;

        /// <summary>
        /// The color of the text on the button
        /// </summary>
        public Color TextColor { get; set; } = Color.White;

        /// <summary>
        /// The animation to use when the button gets clicked
        /// </summary>
        public ButtonPressAnimation PressAnimation { get; set; } = ButtonPressAnimation.SWITCH_COLOR;

        /// <summary>
        /// The text displayed on the button
        /// </summary>
        public string Text { get; set; } = "Button";

        /// <summary>
        /// The horizontal alignment of the text
        /// </summary>
        public HAlign HorizontalAlign = HAlign.LEFT;
        /// <summary>
        /// The vertical alignment of the text
        /// </summary>
        public VAlign VerticalAlign = VAlign.TOP;

        /// <summary>
        /// Called when the button gets clicked
        /// </summary>
        public EventHandler Pressed;

        private bool wasJustPressed = false;

        public Button(BaseWindow parent, int x, int y, int width, int height) : base(parent) {
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

            

            if (!wasJustPressed) {
                ConsoleCanvas.DrawRect(ActualX, ActualY, W, H, IsSelected ? FocusedColor : RegularColor);
                ConsoleCanvas.DrawString(Text, ActualX + xOffset, ActualY + yOffset, W, H, TextColor, IsSelected ? FocusedColor : RegularColor);
            }else {
                if(PressAnimation == ButtonPressAnimation.SWITCH_COLOR) {
                    ConsoleCanvas.DrawRect(ActualX, ActualY, W, H, PressedColor);
                    ConsoleCanvas.DrawString(Text, ActualX + xOffset, ActualY + yOffset, W, H, TextColor, PressedColor);
                }else if(PressAnimation == ButtonPressAnimation.POP_OUT) {
                    ConsoleCanvas.DrawRect(ActualX, ActualY, W, H, PressedColor);
                    ConsoleCanvas.DrawRect(ActualX+1, ActualY+1, W, H, RegularColor);

                    ConsoleCanvas.DrawString(Text, ActualX + xOffset + 1, ActualY + yOffset + 1, W, H, TextColor, RegularColor);
                }
            }
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

        private void InvokePressed() {
            EventHandler handler = Pressed;
            handler?.Invoke(this, new());
        }

        public override Size GetSize() {
            return new(W, H);
        }
    }

    public enum ButtonPressAnimation {
        SWITCH_COLOR,
        POP_OUT
    }
}

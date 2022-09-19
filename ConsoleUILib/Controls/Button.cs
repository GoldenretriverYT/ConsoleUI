using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls {
    public class Button : InteractableControl {
        public int W { get; set; }
        public int H { get; set; }

        public Color FocusedColor { get; set; } = Color.DimGray;
        public Color PressedColor { get; set; } = Color.FromArgb(26, 26, 26);
        public Color RegularColor { get; set; } = Color.SlateGray;
        public Color TextColor { get; set; } = Color.White;
        public ButtonPressAnimation PressAnimation { get; set; } = ButtonPressAnimation.SWITCH_COLOR;

        public string Text { get; set; } = "Button";

        public HAlign HorizontalAlign = HAlign.LEFT;
        public VAlign VerticalAlign = VAlign.TOP;

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
    }

    public enum ButtonPressAnimation {
        SWITCH_COLOR,
        POP_OUT
    }
}

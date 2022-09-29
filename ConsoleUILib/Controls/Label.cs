using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls {
    public class Label : SizedControl {
        /// <summary>
        /// Width of element
        /// </summary>
        public int W { get; set; }
        /// <summary>
        /// Height of element
        /// </summary>
        public int H { get; set; }

        /// <summary>
        /// The color of the text
        /// </summary>
        public Color TextColor { get; set; } = Color.White;

        /// <summary>
        /// The content of the label
        /// </summary>
        public string Text { get; set; } = "Button";

        /// <summary>
        /// Horizontal align of the text
        /// </summary>
        public HAlign HorizontalAlign = HAlign.LEFT;
        /// <summary>
        /// Vertical align of the text
        /// </summary>
        public VAlign VerticalAlign = VAlign.TOP;

        /// <summary>
        /// Called when the label is clicked
        /// </summary>
        public EventHandler Pressed;

        public Label(BaseWindow parent, int x, int y, int width, int height) : base(parent) {
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


            ConsoleCanvas.DrawString(Text, ActualX + xOffset, ActualY + yOffset, W, H, TextColor, ParentWindow.BackgroundColor);
        }


        public override void OnPressed() {
            base.OnPressed();
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
}

using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls {
    public class Button : InteractableControl {
        public int W { get; set; }
        public int H { get; set; }

        public Color FocusedColor { get; set; } = Color.DimGray;
        public Color RegularColor { get; set; } = Color.SlateGray;
        public Color TextColor { get; set; } = Color.White;

        public string Text { get; set; } = "Button";

        public HAlign HorizontalAlign = HAlign.LEFT;
        public VAlign VerticalAlign = VAlign.TOP;


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

            ConsoleCanvas.DrawRect(ActualX, ActualY, W, H, IsSelected ? FocusedColor : RegularColor);
            ConsoleCanvas.DrawString(Text, ActualX + xOffset, ActualY + yOffset, W, H, TextColor, IsSelected ? FocusedColor : RegularColor);
        }
    }
}

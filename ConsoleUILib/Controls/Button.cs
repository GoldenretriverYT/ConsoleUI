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

        public Color FocusedColor { get; set; } = Color.DarkGray;
        public Color RegularColor { get; set; } = Color.Gray;
        public Color TextColor { get; set; } = Color.White;


        public Button(int x, int y, int width, int height) {
            this.X = x;
            this.Y = y;
            this.W = width;
            this.H = height;
        }

        public override void DrawControl() {
            ConsoleCanvas.DrawRect(X, Y, W, H, IsSelected ? FocusedColor : RegularColor);
        }
    }
}

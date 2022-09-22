using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls {
    public abstract class SizedControl : InteractableControl {
        protected SizedControl(BaseWindow parent) : base(parent) {
        }

        public abstract Size GetSize();
    }

    public struct Size {
        public int X;
        public int Y;

        public Size(int x, int y) {
            this.X = x;
            this.Y = y;
        }
    }
}

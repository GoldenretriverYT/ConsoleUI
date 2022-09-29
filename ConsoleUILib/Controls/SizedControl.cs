using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls {
    /// <summary>
    /// Sized control is a control with a fixed width and height.
    /// If you want your element to be clickable by a mouse, you must use this base class.
    /// </summary>
    public abstract class SizedControl : InteractableControl {
        protected SizedControl(BaseWindow parent) : base(parent) {
        }

        /// <summary>
        /// Should return the clickable bounds of the element
        /// </summary>
        /// <returns></returns>
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

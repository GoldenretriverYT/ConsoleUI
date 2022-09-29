using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib {
    /// <summary>
    /// Orientation enum defines the orientation of stuff
    /// </summary>
    public enum Orientation {
        HORIZONTAL,
        VERTICAL
    }

    /// <summary>
    /// All possible overflow states
    /// </summary>
    public enum OverflowState
    {
        HORIZONTAL,
        VERTICAL,
        BOTH,
        NONE,
    }

    /// <summary>
    /// Horizontal alignment possibilities
    /// </summary>
    public enum HAlign {
        LEFT,
        MIDDLE,
        RIGHT,
    }

    /// <summary>
    /// Vertical alignment possibilities
    /// </summary>
    public enum VAlign {
        TOP,
        CENTER,
        BOTTOM
    }
}

using ConsoleUILib.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static ConsoleUILib.Internal.NativeMethods;

namespace ConsoleUILib.Structs {
    /// <summary>
    /// This struct is contained in all mouse events
    /// </summary>
    public struct MouseEvent {
        /// <summary>
        /// The X position of the cursor (in characters)
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y position of the cursor (in characters)
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Current mouse buttons pressed
        /// </summary>
        public MouseState State { get; set; }

        /// <summary>
        /// Contains flags like DoubleClick
        /// </summary>
        public MouseFlags Flags { get; set; }

        /// <summary>
        /// Obsolete, use <see cref="X"/> instead
        /// </summary>
        [Obsolete("This can be replaced with MouseEvent.X")]
        public int CharX => X;
        /// <summary>
        /// Obsolete, use <see cref="Y"/> instead
        /// </summary>
        [Obsolete("This can be replaced with MouseEvent.X")]
        public int CharY => Y;
    }

    [Flags]
    public enum MouseState {
        LMB = 0x0001,
        BUTTON3 = 0x0004,
        BUTTON4 = 0x0008,
        BUTTON5 = 0x0010,
        RMB = 0x0002
    }

    [Flags]
    public enum MouseFlags {
        DOUBLE = 0x0002, // Double Click
        MOUSE_HWHEELED = 0x0008,
        MOUSE_MOVED = 0x0001,
        MOUSE_WHEELED = 0x0004
    }
}

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Internal {
    public class NativeMethods {

        public const Int32 STD_INPUT_HANDLE = -10;

        public const int ENABLE_MOUSE_INPUT = 0x0010;
        public const int ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const int ENABLE_EXTENDED_FLAGS = 0x0080;

        public const Int32 KEY_EVENT = 1;
        public const Int32 MOUSE_EVENT = 2;


        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD {
            [FieldOffset(0)]
            public Int16 EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
        }

        public struct MOUSE_EVENT_RECORD {
            public COORD dwMousePosition;
            public Int32 dwButtonState;
            public Int32 dwControlKeyState;
            public Int32 dwEventFlags;
        }

        public struct COORD {
            public UInt16 X;
            public UInt16 Y;
        }

        [Flags] public enum ControlKeyState {
            CAPSLOCK_ON = 0x0080,
            ENHANCED_KEY = 0x0100,
            LEFT_ALT_PRESSED = 0x0002,
            LEFT_CTRL_PRESSED = 0x0008,
            NUMLOCK_ON = 0x0020,
            RIGHT_ALT_PRESSED = 0x0001,
            RIGHT_CTRL_PRESSED = 0x0004,
            SCROLLOCK_ON = 0x0040,
            SHIFT_PRESSED = 0x0010,
        }


        [StructLayout(LayoutKind.Explicit)]
        public struct KEY_EVENT_RECORD {
            [FieldOffset(0)]
            [MarshalAsAttribute(UnmanagedType.Bool)]
            public Boolean bKeyDown;
            [FieldOffset(4)]
            public UInt16 wRepeatCount;
            [FieldOffset(6)]
            public UInt16 wVirtualKeyCode;
            [FieldOffset(8)]
            public UInt16 wVirtualScanCode;
            [FieldOffset(10)]
            public Char UnicodeChar;
            [FieldOffset(10)]
            public Byte AsciiChar;
            [FieldOffset(12)]
            public Int32 dwControlKeyState;
        };


        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean GetConsoleMode(ConsoleHandle hConsoleHandle, ref int lpMode);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        public static extern ConsoleHandle GetStdHandle(Int32 nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean ReadConsoleInput(ConsoleHandle hConsoleInput, ref INPUT_RECORD lpBuffer, UInt32 nLength, ref UInt32 lpNumberOfEventsRead);

        [DllImportAttribute("kernel32.dll", SetLastError = true)]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern Boolean SetConsoleMode(ConsoleHandle hConsoleHandle, int dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetNumberOfConsoleInputEvents(
        ConsoleHandle hConsoleInput,
        out uint lpcNumberOfEvents
        );

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfoEx(
        ConsoleHandle hConsoleOutput,
        ref CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfo
        );
    }

    public class ConsoleHandle : SafeHandleMinusOneIsInvalid {
        public ConsoleHandle() : base(false) { }

        protected override bool ReleaseHandle() {
            return true; //releasing console handle is not our business
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
        public int Height => Math.Abs(Top - Bottom);
        public int Width => Math.Abs(Left - Right);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_SCREEN_BUFFER_INFO_EX {
        public uint cbSize;
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;

        public ushort wPopupAttributes;
        public bool bFullscreenSupported;

        internal COLORREF black;
        internal COLORREF darkBlue;
        internal COLORREF darkGreen;
        internal COLORREF darkCyan;
        internal COLORREF darkRed;
        internal COLORREF darkMagenta;
        internal COLORREF darkYellow;
        internal COLORREF gray;
        internal COLORREF darkGray;
        internal COLORREF blue;
        internal COLORREF green;
        internal COLORREF cyan;
        internal COLORREF red;
        internal COLORREF magenta;
        internal COLORREF yellow;
        internal COLORREF white;

        // has been a while since I did this, test before use
        // but should be something like:
        //
        // [MarshalAs(UnmanagedType.ByValArray, SizeConst=16)]
        // public COLORREF[] ColorTable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF {
        public uint ColorDWORD;

        public COLORREF(System.Drawing.Color color) {
            ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }

        public System.Drawing.Color GetColor() {
            return System.Drawing.Color.FromArgb((int)(0x000000FFU & ColorDWORD),
           (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
        }

        public void SetColor(System.Drawing.Color color) {
            ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD {
        public short X;
        public short Y;

        public COORD(short X, short Y) {
            this.X = X;
            this.Y = Y;
        }
    };

    public struct SMALL_RECT {

        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }
}

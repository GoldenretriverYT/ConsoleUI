﻿using ConsoleUILib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleUILib.Internal.NativeMethods;

namespace ConsoleUILib.Structs {
    public struct MouseEvent {
        public int X { get; set; }
        public int Y { get; set; }
        public MouseState State { get; set; }
        public MouseFlags Flags { get; set; }

        public int CharX => GetCharX();
        public int CharY => GetCharY();

        private int GetCharX() {
            RECT cRect = GetConsoleRect();
            RECT charRect = GetRectPerChar(cRect);

            return (X - cRect.Left) / charRect.Width;
        }

        private int GetCharY() {
            RECT cRect = GetConsoleRect();
            RECT charRect = GetRectPerChar(cRect);

            return (Y - cRect.Top) / charRect.Height;
        }

        private RECT GetConsoleRect() {
            NativeMethods.GetWindowRect(UIManager.handle, out RECT rect);
            return rect;
        }

        private RECT GetRectPerChar(RECT rect) {
            return new RECT() { Top = 0, Left = 0, Bottom = Console.WindowHeight / rect.Height, Right = Console.WindowWidth / rect.Width };
        }
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
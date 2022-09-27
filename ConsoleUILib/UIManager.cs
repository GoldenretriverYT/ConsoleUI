using ConsoleUILib.Internal;
using ConsoleUILib.Structs;
using ConsoleUILib.Window;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace ConsoleUILib
{
    public class UIManager
    {
        private static List<BaseWindow> Windows { get; } = new();
        public static BaseWindow FocusedWindow { get; set; }
        public static ConsoleHandle handle;
        public static CONSOLE_SCREEN_BUFFER_INFO_EX cBuf;
        public static COORD MousePosition { get; set; }
        public static bool ClearScreenOnRedraw { get; set; }

        public static void Start()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
            handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            string strCmdText = "/C cls";
            System.Diagnostics.Process.Start("CMD.exe", strCmdText).WaitForExit();

            int mode = 0;
            if (!(NativeMethods.GetConsoleMode(handle, ref mode))) { throw new Exception(); }

            mode |= NativeMethods.ENABLE_MOUSE_INPUT;
            mode &= ~NativeMethods.ENABLE_QUICK_EDIT_MODE;
            mode |= NativeMethods.ENABLE_EXTENDED_FLAGS;

            if (!(NativeMethods.SetConsoleMode(handle, mode))) { throw new Exception(); }
            Stopwatch sw = new();

            RenderWindows();

            while(true)
            {
                Console.ResetColor();

                NativeMethods.GetNumberOfConsoleInputEvents(handle, out uint eventsAvailable);

                if (eventsAvailable > 0) {
                    var record = new NativeMethods.INPUT_RECORD();
                    uint recordLen = 0;

                    if (!(NativeMethods.ReadConsoleInput(handle, ref record, 1, ref recordLen))) { throw new Exception(); }

                    Console.SetCursorPosition(0, 0);
                    switch (record.EventType) {
                        case NativeMethods.MOUSE_EVENT:
                            MouseEvent me = new();
                            me.State = (MouseState)record.MouseEvent.dwButtonState;
                            me.X = record.MouseEvent.dwMousePosition.X;
                            me.Y = record.MouseEvent.dwMousePosition.Y;
                            me.Flags = (MouseFlags)record.MouseEvent.dwEventFlags;

                            MousePosition = new() { X = (short)me.CharX, Y = (short)me.CharY };

                            FocusedWindow.HandleMouse(me);
                            break;

                        case NativeMethods.KEY_EVENT: 
                            NativeMethods.ControlKeyState cks = (NativeMethods.ControlKeyState)record.KeyEvent.dwControlKeyState;
                            ConsoleKeyInfo cki = new(record.KeyEvent.UnicodeChar, (ConsoleKey)record.KeyEvent.wVirtualKeyCode, cks.HasFlag(NativeMethods.ControlKeyState.SHIFT_PRESSED), cks.HasFlag(NativeMethods.ControlKeyState.LEFT_ALT_PRESSED) || cks.HasFlag(NativeMethods.ControlKeyState.RIGHT_ALT_PRESSED), cks.HasFlag(NativeMethods.ControlKeyState.LEFT_CTRL_PRESSED) || cks.HasFlag(NativeMethods.ControlKeyState.RIGHT_CTRL_PRESSED));
                            if(record.KeyEvent.bKeyDown == true) FocusedWindow.HandleKeyDown(cki);
                            break;
                        default: break;
                    }

                }

                sw.Restart();

                if (ClearScreenOnRedraw) {
                    Console.BufferWidth = Console.WindowWidth;
                    Console.BufferHeight = Console.WindowHeight;
                    ClearScreenOnRedraw = false;
                    Console.Clear();
                }

                RenderWindows();

                foreach(BaseWindow window in Windows) {
                    window.HandleRenderDone();
                }

                Console.Title = "- | Frame Render Time: " + Math.Floor((double)sw.Elapsed.TotalMilliseconds) + "ms | Theoretical possible FPS: " + Math.Floor(1000d / sw.Elapsed.TotalMilliseconds) + " (limited to 30)";
                Thread.Sleep(1000 / 30);
            }
        }

        private static void RenderWindows() {
            foreach (BaseWindow window in Windows) {
                window.HandleBeforeDraw();
                window.DrawWindow();
                Console.ResetColor();
            }
        }

        public static void ForceRender(bool redrawEverything = false) {
            if (redrawEverything) {
                Console.BufferWidth = Console.WindowWidth;
                Console.BufferHeight = Console.WindowHeight;
                Console.Clear();
            }
            RenderWindows();
        }

        public static void AddWindow(BaseWindow window) {
            Windows.Add(window);
            FocusedWindow = window;
        }
    }
}
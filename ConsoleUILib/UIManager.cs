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
        private static List<BaseWindow> pendingWindowRemovals = new();

        private static ConsoleHandle handle;
        private static CONSOLE_SCREEN_BUFFER_INFO_EX cBuf;

        /// <summary>
        /// Current mouse cursor position in character coordinates
        /// </summary>
        public static COORD MousePosition { get; set; }

        /// <summary>
        /// The currently focused window. Window focus changes when the users clicks the window.
        /// </summary>
        public static BaseWindow FocusedWindow { get; set; }

        // Flags for UI Manager
        /// <summary>
        /// If set to true, it will clear the screen before rendering the next frame.
        /// It is automatically set to false after that.
        /// </summary>
        public static bool ClearScreenOnRedraw { get; set; }

        /// <summary>
        /// If set to true, the windows focus can change by using the cursor.
        /// </summary>
        public static bool AllowChangeFocusedWindow { get; set; } = true;


        // All stuff related to framerate
        /// <summary>
        /// Contains the render times of the last <see cref="FrameTimesHistoryLength"/> frames
        /// </summary>
        public static List<double> FrameTimes { get; set; } = new();

        /// <summary>
        /// Defines how many frames should be kept in <see cref="FrameTimes"/> list.
        /// Reducing whilst running will not actually get rid of the excess entries,
        /// you will have to clear the list for that
        /// </summary>
        public static int FrameTimesHistoryLength { get; set; } = 15;

        /// <summary>
        /// Sets the max frame rate. Increasing it too high will cause flickering.
        /// </summary>
        public static int MaxFrameRate { get; set; } = 30;



        /// <summary>
        /// Start the UIManager rendering & input thread
        /// </summary>
        /// <exception cref="Exception">Unexpected error, possibly on wrong platform</exception>
        public static void Start()
        {
            Console.CursorVisible = false;
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
            handle = NativeMethods.GetStdHandle(NativeMethods.STD_INPUT_HANDLE);

            NativeMethods.SetConsoleCP(65001);

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
                sw.Restart();

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

                            if (me.State.HasFlag(MouseState.LMB) && AllowChangeFocusedWindow) // Left mouse button most be pressed. Also make sure that
                            {                                                                 // you can not drag more than one window by only allowing
                                foreach (BaseWindow window in Windows)                        // changing focused window when no window is being dragged
                                {
                                    if (MousePosition.X > window.X && MousePosition.X < window.X + window.Width &&
                                        MousePosition.Y >= window.Y && MousePosition.Y < window.Y + window.Height)
                                    {
                                        if (FocusedWindow is CustomWindow cwin)
                                        {
                                            if (cwin.Focused != null) cwin.Focused.IsSelected = false;
                                            cwin.FocusedIndex = -1;
                                        }

                                        FocusedWindow = window;
                                        Windows.Remove(window);
                                        Windows.Add(window);
                                        break;
                                    }
                                }
                            }

                            if(FocusedWindow is not null) FocusedWindow.HandleMouse(me);

                            break;

                        case NativeMethods.KEY_EVENT: 
                            NativeMethods.ControlKeyState cks = (NativeMethods.ControlKeyState)record.KeyEvent.dwControlKeyState;
                            ConsoleKeyInfo cki = new(record.KeyEvent.UnicodeChar, (ConsoleKey)record.KeyEvent.wVirtualKeyCode, cks.HasFlag(NativeMethods.ControlKeyState.SHIFT_PRESSED), cks.HasFlag(NativeMethods.ControlKeyState.LEFT_ALT_PRESSED) || cks.HasFlag(NativeMethods.ControlKeyState.RIGHT_ALT_PRESSED), cks.HasFlag(NativeMethods.ControlKeyState.LEFT_CTRL_PRESSED) || cks.HasFlag(NativeMethods.ControlKeyState.RIGHT_CTRL_PRESSED));
                            if(record.KeyEvent.bKeyDown == true) if (FocusedWindow is not null) FocusedWindow.HandleKeyDown(cki);
                            break;
                        default: break;
                    }

                }

                if (ClearScreenOnRedraw) {
                    Console.BufferWidth = Console.WindowWidth;
                    Console.BufferHeight = Console.WindowHeight;
                    ClearScreenOnRedraw = false;
                    Console.ResetColor();
                    Console.Clear();
                }

                RenderWindows();

                foreach(BaseWindow window in Windows) {
                    window.HandleRenderDone();
                }

                if (FrameTimes.Count > FrameTimesHistoryLength) FrameTimes.RemoveAt(0);
                FrameTimes.Add(sw.Elapsed.TotalMilliseconds);

                double sum = 0;
                foreach(double ms in FrameTimes) sum += ms;

                Console.Title = "- | Avg Render Time (last 15 frames): " + Math.Floor(sum/FrameTimes.Count) + "ms | Theoretical possible FPS: " + Math.Floor(1000d / (sum / FrameTimes.Count)) + " (capped at " + MaxFrameRate + ") | Actual FPS: " + Math.Min(MaxFrameRate, Math.Floor(1000d / (sum / FrameTimes.Count)));
                Thread.Sleep(1000 / MaxFrameRate);
            }
        }

        private static void RenderWindows() {
            foreach(BaseWindow window in pendingWindowRemovals)
            {
                if (FocusedWindow == window) FocusedWindow = null;
                Windows.Remove(window);
                ClearScreenOnRedraw = true;
            }

            pendingWindowRemovals.Clear();

            foreach (BaseWindow window in Windows) {
                window.HandleBeforeDraw();
                window.DrawWindow();
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Will force an immediate re-render of all windows
        /// </summary>
        /// <param name="redrawEverything">If true, it will clear the screen before drawing.</param>
        public static void ForceRender(bool redrawEverything = false) {
            if (redrawEverything) {
                Console.BufferWidth = Console.WindowWidth;
                Console.BufferHeight = Console.WindowHeight;
                Console.ResetColor();
                Console.Clear();
            }
            RenderWindows();
        }


        /// <summary>
        /// Add a window to the UIManager.
        /// It will not be drawn until added using this method
        /// </summary>
        /// <param name="window">The window to add</param>
        public static void AddWindow(BaseWindow window) {
            Windows.Add(window);
            FocusedWindow = window;
        }
        
        /// <summary>
        /// Remove a window from the UIManager.
        /// </summary>
        /// <param name="window">The window to remove</param>
        public static void RemoveWindow(BaseWindow window)
        {
            pendingWindowRemovals.Add(window);
        }
    }
}
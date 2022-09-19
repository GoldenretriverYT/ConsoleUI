using ConsoleUILib.Window;
using System.Diagnostics;

namespace ConsoleUILib
{
    public class UIManager
    {
        private static List<BaseWindow> Windows { get; } = new();
        public static BaseWindow FocusedWindow { get; set; }

        public static void Start()
        {
            Console.CursorVisible = false;
            Stopwatch sw = new();

            RenderWindows();

            while(true)
            {
                Console.ResetColor();

                if (Console.KeyAvailable) {
                    ConsoleKeyInfo cki = Console.ReadKey();
                    FocusedWindow.HandleKeyDown(cki);
                }

                sw.Restart();
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
                window.DrawWindow();
                Console.ResetColor();
            }
        }

        public static void ForceRender() {
            RenderWindows();
        }

        public static void AddWindow(BaseWindow window) {
            Windows.Add(window);
            FocusedWindow = window;
        }
    }
}
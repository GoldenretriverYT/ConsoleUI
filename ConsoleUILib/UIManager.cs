using ConsoleUILib.Window;

namespace ConsoleUILib
{
    public class UIManager
    {
        private List<BaseWindow> Windows { get; init; }
        public BaseWindow FocusedWindow { get; set; }

        public UIManager()
        {
            this.Windows = new();
        }

        public void Start()
        {
            RenderWindows();

            while(true)
            {
                Console.ResetColor();

                ConsoleKeyInfo cki = Console.ReadKey();
                FocusedWindow.HandleKeyDown(cki);

                RenderWindows();
            }
        }

        private void RenderWindows() {
            foreach (BaseWindow window in Windows) {
                window.DrawWindow();
                Console.ResetColor();
            }
        }

        public void AddWindow(BaseWindow window) {
            Windows.Add(window);
            FocusedWindow = window;
        }
    }
}
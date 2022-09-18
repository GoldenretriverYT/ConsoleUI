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
            while(true)
            {
                Console.ResetColor();

                foreach(BaseWindow window in Windows)
                {
                    window.DrawWindow();
                    Console.ResetColor();
                }

                Console.ReadKey();
            }
        }

        public void AddWindow(BaseWindow window) {
            Windows.Add(window);
            FocusedWindow = window;
        }
    }
}
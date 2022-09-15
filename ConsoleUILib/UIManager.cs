using ConsoleUILib.Window;

namespace ConsoleUILib
{
    public class UIManager
    {
        public List<BaseWindow> Windows { get; init; }

        public UIManager()
        {
            this.Windows = new();
        }

        public void Start()
        {
            while(true)
            {
                foreach(BaseWindow window in Windows)
                {
                    window.DrawWindow();
                }

                Console.ReadKey();
            }
        }
    }
}
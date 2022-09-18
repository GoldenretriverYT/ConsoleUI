using ConsoleUILib.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Window
{
    public abstract class BaseWindow
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }

        public virtual void DrawWindow()
        {
            if(Width < 1 || Height < 1)
            {
                throw new DrawException(this, "Width or height is below 1");
            }

            if (X + Width > 100 || Y + Height > 100)
            {
                throw new DrawException(this, "The size and position would render out of bounds.");
            }else
            {
                if (Console.WindowWidth < X + Width) Console.WindowWidth = X + Width;
                if (Console.WindowHeight < Y + Height) Console.WindowHeight = Y + Height;
            }

            ConsoleCanvas.DrawRectGradient(X, Y, Width, Height, Orientation.HORIZONTAL, Color.Red, Color.Purple);
            ConsoleCanvas.DrawRect(X, Y, Width, 1, Color.DarkGray);
            ConsoleCanvas.DrawString(Title, X, Y, Width, 1, Color.White, Color.DarkGray);
        }
    }
}

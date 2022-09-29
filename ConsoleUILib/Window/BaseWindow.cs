using ConsoleUILib.Exceptions;
using ConsoleUILib.Structs;
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

        public Color BackgroundColor { get; set; } = Color.Gray;

        public event EventHandler RenderDone;
        public event EventHandler BeforeDraw;

        public virtual void DrawWindow()
        {
            if(Width < 1 || Height < 1)
            {
                throw new DrawException(this, "Width or height is below 1");
            }

            if (X + Width > Console.WindowWidth || Y + Height > Console.WindowHeight)
            {
                throw new DrawException(this, "The size and position would render out of bounds.");
            }else
            {
                if (Console.WindowWidth < X + Width) Console.WindowWidth = X + Width;
                if (Console.WindowHeight < Y + Height) Console.WindowHeight = Y + Height;
            }

            ConsoleCanvas.DrawRect(X, Y, Width, Height, Color.Gray);
            ConsoleCanvas.DrawRect(X, Y, Width, 1, Color.DarkGray);
            ConsoleCanvas.DrawString(Title, X, Y, Width, 1, Color.White, Color.DarkGray);
        }

        public virtual void HandleKeyDown(ConsoleKeyInfo key) {

        }

        public virtual void HandleMouse(MouseEvent key) {

        }

        /// <summary>
        /// This invokes the <see cref="RenderDone"/> event
        /// </summary>
        public virtual void HandleRenderDone() {
            EventHandler ev = RenderDone;
            ev?.Invoke(this, new());
        }

        /// <summary>
        /// This invokes the <see cref="BeforeDraw"/> event
        /// </summary>
        public virtual void HandleBeforeDraw() {
            EventHandler ev = BeforeDraw;
            ev?.Invoke(this, new());
        }
    }
}

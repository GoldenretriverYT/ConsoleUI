using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls
{
    public abstract class BaseControl
    {
        private static Random rnd = new();

        /// <summary>
        /// X position of the control within the window. For custom controls drawing, use <see cref="ActualX"/>
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y position of the control within the window. For custom controls drawing, use <see cref="ActualY"/>
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// X position of the control within the whole console
        /// </summary>
        public int ActualX => X + ParentWindow.X;
        /// <summary>
        /// Y position of the control within the whole console
        /// </summary>
        public int ActualY => Y + ParentWindow.Y+1;

        /// <summary>
        /// The parent window of the control
        /// </summary>
        public BaseWindow ParentWindow { get; set; }

        /// <summary>
        /// A randomized id unique for every control.
        /// </summary>
        public int RandomizedID { get; private set; }

        public virtual void DrawControl()
        {

        }

        public BaseControl(BaseWindow parent) {
            ParentWindow = parent;
            RandomizedID = rnd.Next(Int32.MinValue, Int32.MaxValue);
        }
    }
}

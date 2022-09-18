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
        public int X { get; set; }
        public int Y { get; set; }

        protected int ActualX => X + ParentWindow.X;
        protected int ActualY => Y + ParentWindow.Y;

        public BaseWindow ParentWindow { get; set; }

        public virtual void DrawControl()
        {

        }

        public BaseControl(BaseWindow parent) {
            ParentWindow = parent;
        }
    }
}

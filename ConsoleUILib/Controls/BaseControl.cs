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

        public virtual void DrawControl()
        {

        }
    }
}

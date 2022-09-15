using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Exceptions
{
    internal class DrawException : Exception
    {
        public DrawException(BaseWindow from, string msg) : base("Drawing window " + from.Title + " failed: " + msg)
        {
            
        }
    }
}

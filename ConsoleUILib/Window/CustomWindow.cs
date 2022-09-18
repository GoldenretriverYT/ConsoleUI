using ConsoleUILib.Controls;
using ConsoleUILib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Window
{
    public class CustomWindow : BaseWindow
    {
        public List<BaseControl> Controls { get; init; }

        public CustomWindow(int x, int y, int w, int h)
        {
            this.Controls = new();
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }

        public void AddControl(BaseControl control)
        {
            this.Controls.Add(control);
        }

        public override void DrawWindow() {
            base.DrawWindow();

            foreach(BaseControl ctrl in Controls) {
                ctrl.DrawControl();
            }
        }

        public void FindNextInteractable(int lastIdx, out BaseControl outCtrl, out int outIdx)
        {
            int idx = lastIdx + 1;

            foreach(BaseControl control in Controls.Skip(lastIdx))
            {
                if(control is InteractableControl)
                {
                    outCtrl = control;
                    outIdx = idx; 
                }
                idx++;
            }

            if (lastIdx != 0)
                FindNextInteractable(0, out outCtrl, out outIdx);
            else
            {
                outCtrl = null;
                outIdx = 0;
            }
        }
    }
}

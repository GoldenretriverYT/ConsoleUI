using ConsoleUILib.Controls;
using ConsoleUILib.Exceptions;
using ConsoleUILib.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Window
{
    public class CustomWindow : BaseWindow
    {
        public List<BaseControl> Controls { get; init; }
        public int FocusedIndex { get; set; } = -1;
        public InteractableControl? Focused => (FocusedIndex == -1 ? null : Controls[FocusedIndex] as InteractableControl);

        private bool isMouseDown = false;
        private bool isDragging = false;
        private short draggingOffset = 0;

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

            foreach(BaseControl control in Controls.Skip(lastIdx+1))
            {
                if(control is InteractableControl)
                {
                    outCtrl = control;
                    outIdx = idx;
                    return;
                }
                idx++;
            }

            if (lastIdx != 0)
                FindNextInteractable(-1, out outCtrl, out outIdx);
            else
            {
                outCtrl = null;
                outIdx = -1;
            }
        }

        public override void HandleKeyDown(ConsoleKeyInfo key) {
            base.HandleKeyDown(key);
            Debug.WriteLine("Window " + Title + " handling key press " + key.Key.ToString() + " (SHIFT: " + key.Modifiers.HasFlag(ConsoleModifiers.Shift) + ")");

            if(key.Key == ConsoleKey.Tab) {
                FindNextInteractable(FocusedIndex, out _, out int newIdx);
                if (newIdx != -1) {
                    if(Focused != null) Focused.IsSelected = false;
                    FocusedIndex = newIdx;
                    if (Focused != null) Focused.IsSelected = true;
                    return;
                }else {
                    Debug.WriteLine("Wasn't able to find new selectable object");
                }
            }

            if(key.Key == ConsoleKey.Enter && Focused != null && Focused.EnterShouldPress) {
                Focused.OnPressed();
                return;
            }

            Debug.WriteLine("Calling OnKeyDown for " + Focused);
            Focused?.OnKeyDown(key);
        }

        public override void HandleMouse(MouseEvent key) {
            base.HandleMouse(key);

            if (key.State.HasFlag(MouseState.LMB)) {
                isMouseDown = true;

                if(key.CharX >= X && key.CharX < X+Width &&
                    key.CharY >= Y && key.CharY < Y+1 ) {
                    if(!isDragging) draggingOffset = (short)(key.CharX - X);
                    isDragging = true;
                    UIManager.AllowChangeFocusedWindow = false;
                }

                if (isDragging) return;

                foreach (BaseControl control in Controls) {
                    if (control is not SizedControl iCtrl) continue; // OMG THANKS KNELIS (on SO) FOR THIS KNOWLEDGE

                    if (key.CharX >= iCtrl.ActualX && key.CharX < (iCtrl.ActualX + iCtrl.GetSize().X) &&
                        key.CharY >= iCtrl.ActualY && key.CharY < (iCtrl.ActualY + iCtrl.GetSize().Y)) {
                        iCtrl.OnPressed();

                        if(Focused != null) Focused.IsSelected = false;
                        FocusedIndex = Controls.IndexOf(iCtrl);
                        Focused.IsSelected = true;
                    }
                }
            }else if(isMouseDown) {
                isMouseDown = false;
            }
        }

        public override void HandleRenderDone() {
            base.HandleRenderDone();

            
        }

        public override void HandleBeforeDraw() {
            if (isDragging)
            {
                if (isMouseDown)
                {
                    X = Math.Clamp(UIManager.MousePosition.X - draggingOffset, 0, Console.WindowWidth - Width);
                    Y = Math.Clamp(UIManager.MousePosition.Y, 0, Console.WindowHeight - Height);
                    UIManager.ClearScreenOnRedraw = true;
                }
                else
                {
                    isDragging = false;
                    UIManager.AllowChangeFocusedWindow = true;
                }
            }
        }
    }
}

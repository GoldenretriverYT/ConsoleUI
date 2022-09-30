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
        /// <summary>
        /// List of controls. However, it is recommended to use <see cref="AddControl(BaseControl)"/> to add controls instead.
        /// </summary>
        public List<BaseControl> Controls { get; init; }

        /// <summary>
        /// The index of the focused element
        /// </summary>
        public int FocusedIndex { get; set; } = -1;

        /// <summary>
        /// The focused element. If index is -1, it will return null
        /// </summary>
        public InteractableControl? Focused => (FocusedIndex == -1 ? null : Controls[FocusedIndex] as InteractableControl);

        private bool isMouseDown = false;
        private bool isDragging = false;
        private short draggingOffset = 0;

        /// <summary>
        /// Create a new window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public CustomWindow(int x, int y, int w, int h)
        {
            this.Controls = new();
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }

        /// <summary>
        /// Add a control to the window
        /// </summary>
        /// <param name="control">Control to add</param>
        public void AddControl(BaseControl control)
        {
            this.Controls.Add(control);
        }

        /// <summary>
        /// Draws the window. You might want to use <see cref="UIManager.ForceRender(bool)"/> instead.
        /// </summary>
        public override void DrawWindow() {
            base.DrawWindow();

            foreach(BaseControl ctrl in Controls.ToArray()) {
                ctrl.DrawControl();
            }
        }

        /// <summary>
        /// This method finds the next interactable element.
        /// This is primarily used internally
        /// </summary>
        /// <param name="lastIdx"></param>
        /// <param name="outCtrl"></param>
        /// <param name="outIdx"></param>
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

        /// <summary>
        /// Called when a new KeyEvent is detected. You can call it manually to simulate key presses.
        /// </summary>
        /// <param name="key"></param>
        public override void HandleKeyDown(ConsoleKeyInfo key) {
            base.HandleKeyDown(key);
            Debug.WriteLine("Window " + Title + " handling key press " + key.Key.ToString() + " (CHAR: " + key.KeyChar + "; SHIFT: " + key.Modifiers.HasFlag(ConsoleModifiers.Shift) + ")");

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

        /// <summary>
        /// Called when a new MouseEvent is detected. You can call it manually to simulate mouse clicks.
        /// </summary>
        /// <param name="key"></param>
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

                foreach (BaseControl control in Controls.ToArray()) { // Copy the list so we can modify it whilst rendering
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

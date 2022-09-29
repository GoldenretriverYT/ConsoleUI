using ConsoleUILib.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls
{
    public abstract class InteractableControl : BaseControl
    {
        /// <summary>
        /// Decides if pressing ENTER should call <see cref="OnPressed"/> - for example, a text field has this set to false but a button to true
        /// </summary>
        public virtual bool EnterShouldPress => true;

        /// <summary>
        /// Called when the user clicks it with the mouse of, if <see cref="EnterShouldPress"/> is enabled, presses ENTER
        /// </summary>
        public virtual void OnPressed() { }

        /// <summary>
        /// Called when the user presses any key.
        /// </summary>
        /// <param name="key"></param>
        public virtual void OnKeyDown(ConsoleKeyInfo key) { }

        /// <summary>
        /// This is true, if the element is currently focused. <b>Setting it manually might cause conflicts. Do this at your own risk.</b>
        /// </summary>
        public bool IsSelected { get; set; }

        public InteractableControl(BaseWindow parent) : base(parent) { }
    }
}

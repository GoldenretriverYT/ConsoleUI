﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUILib.Controls
{
    public abstract class InteractableControl : BaseControl
    {
        public virtual bool EnterShouldPress => true;
        public virtual void OnPressed() { }
        public virtual void OnKeyDown() { }

        public bool IsSelected { get; set; }
    }
}

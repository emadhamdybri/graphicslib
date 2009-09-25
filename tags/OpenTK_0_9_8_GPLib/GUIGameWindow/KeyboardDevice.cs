using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace GUIGameWindow
{
    public delegate void KeyDownEvent(KeyboardDevice sender, Keys key);
    public delegate void KeyUpEvent(KeyboardDevice sender, Keys key);
   
    public class KeyboardDevice
    {
        Dictionary<Keys, bool> keymap = new Dictionary<Keys,bool>();

        public event KeyUpEvent KeyUp;
        public event KeyDownEvent KeyDown;

        public bool this[Keys key]
        {
            get 
            {
                if (!keymap.ContainsKey(key))
                    return false;
                else
                    return keymap[key]; 
            }
        }

        public KeyboardDevice(Control ctl)
        {
            ctl.KeyDown += new KeyEventHandler(InternalDown);
            ctl.KeyUp += new KeyEventHandler(InternalUp);
        }

        protected void InternalDown(Object sender, KeyEventArgs args)
        {
            keymap[args.KeyCode] = true;
            if (KeyDown != null)
                KeyDown(this, args.KeyCode);
        }

        protected void InternalUp(Object sender, KeyEventArgs args)
        {
            keymap[args.KeyCode] = false;
            if (KeyUp != null)
                KeyUp(this, args.KeyCode);
        }
    }

}

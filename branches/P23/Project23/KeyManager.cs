using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Input;
using OpenTK;

namespace Project23
{
    public enum KeyEvent
    {
        None,
        Quit,
        Accept,
        Forward,
        Backward,
        Left,
        Right,
        Fire,
        Launch,
        Activate,
        Chat1,
        Chat2,
        Chat3,
        Chat4,
        Menu,
        StartChat,
        ReplyTo,
        TeamChat,
        PlayerList,
    }

    public class KeyManager
    {
        public class KeyEventItem
        {
            public KeyEvent Event = KeyEvent.None;
            public OpenTK.Input.Key Key = OpenTK.Input.Key.Unknown;
        }

        public List<KeyEventItem> KeyEvents = new List<KeyEventItem>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Dictionary<KeyEvent, OpenTK.Input.Key> KeyEventMap = new Dictionary<KeyEvent, OpenTK.Input.Key>();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        KeyboardHandler keyboard;

        public KeyManager ( )
        {
        }

        public void SetDefaults ()
        {
            AddKeyConditional(KeyEvent.Menu, Key.Escape);
            AddKeyConditional(KeyEvent.Accept, Key.End);
            AddKeyConditional(KeyEvent.Forward, Key.W);
            AddKeyConditional(KeyEvent.Backward, Key.S);
            AddKeyConditional(KeyEvent.Left, Key.A);
            AddKeyConditional(KeyEvent.Right, Key.D);
            AddKeyConditional(KeyEvent.Activate, Key.Tab);
            AddKeyConditional(KeyEvent.Chat1, Key.F1);
            AddKeyConditional(KeyEvent.Chat2, Key.F2);
            AddKeyConditional(KeyEvent.Chat3, Key.F3);
            AddKeyConditional(KeyEvent.Chat4, Key.F4);
            AddKeyConditional(KeyEvent.StartChat, Key.Enter);
            AddKeyConditional(KeyEvent.PlayerList, Key.Tilde);
            Serialize();
        }

        protected void AddKeyConditional ( KeyEvent e, Key key )
        {
            if (!KeyEventMap.ContainsKey(e))
                KeyEventMap.Add(e, key);
        }

        public void Deserialize ()
        {
            KeyEventMap.Clear();
            foreach ( KeyEventItem item in KeyEvents )
                KeyEventMap.Add(item.Event, item.Key);
        }

        public void Serialize ()
        {
            KeyEvents.Clear();

            foreach(KeyValuePair<KeyEvent,OpenTK.Input.Key> item in KeyEventMap)
            {
                KeyEventItem i = new KeyEventItem();
                i.Event = item.Key;
                i.Key = item.Value;
                KeyEvents.Add(i);
            }
        }

        public void SetKeyboard ( KeyboardHandler k )
        {
            keyboard = k;
        }

        public bool Keydown ( KeyEvent key )
        {
            if (!KeyEventMap.ContainsKey(key))
                return false;
            return keyboard.KeyDown(KeyEventMap[key]);
        }
    }
}

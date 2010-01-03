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
            KeyEventMap.Add(KeyEvent.Menu, Key.Escape);
            KeyEventMap.Add(KeyEvent.Accept, Key.End);
            KeyEventMap.Add(KeyEvent.Forward, Key.W);
            KeyEventMap.Add(KeyEvent.Backward, Key.S);
            KeyEventMap.Add(KeyEvent.Left, Key.A);
            KeyEventMap.Add(KeyEvent.Right, Key.D);
            KeyEventMap.Add(KeyEvent.Activate, Key.Tab);
            KeyEventMap.Add(KeyEvent.Chat1, Key.F1);
            KeyEventMap.Add(KeyEvent.Chat2, Key.F2);
            KeyEventMap.Add(KeyEvent.Chat3, Key.F3);
            KeyEventMap.Add(KeyEvent.Chat4, Key.F4);
            KeyEventMap.Add(KeyEvent.StartChat, Key.Enter);
            KeyEventMap.Add(KeyEvent.PlayerList, Key.Tilde);
            Serialize();
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

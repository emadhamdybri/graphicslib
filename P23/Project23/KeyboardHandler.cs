using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenTK;
using OpenTK.Input;

namespace Project23
{
    public delegate void NewLineHandler ( object sender, Key key );

    public class KeyboardHandler
    {
        public bool TextMode
        {
            get { return textMode; }
            set
            { 
                if (!value && deleteTimer != null)
                {
                    deleteTimer.Dispose();
                    deleteTimer = null;
                }
                textMode = value;
            }
        }

        protected bool textMode = false;
        public string CurrentText
        {
            get { lock (currentText) { return string.Copy(currentText); } }
            set { lock (currentText) { currentText = value; } }
        }


        string currentText = string.Empty;

        public event NewLineHandler TextModeNewline;

        public bool FlushKeysOnUp = false;

        List<Key> Keys = new List<Key>();

        bool Shift = false;
        bool CapsLock = false;

        public int DeleteRepeatTime = 100;
        public int DeleteRepeatDelay = 400;

        Timer deleteTimer = null;

        public KeyboardHandler ( GameWindow gamewindow )
        {
            gamewindow.Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(Keyboard_KeyDown);
            gamewindow.Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(Keyboard_KeyUp);
        }

        public bool KeyDown( Key key )
        {
            return Keys.Contains(key);
        }

        public Key[] GetKeys ()
        {
            return Keys.ToArray();
        }

        public void FlushKeys ()
        {
            Keys.Clear();
        }

        public void FlushText ()
        {
            lock(currentText)
            {
                currentText = string.Empty;
            }
        }

        string toKey (Key key)
        {
            if (key >= Key.Number0 && key <= Key.Number9 && !Shift)
                return (((int)key) - (int)Key.Number0).ToString();
            else if (key >= Key.Keypad0 && key <= Key.Keypad9)
                return (((int)key) - (int)Key.Keypad0).ToString();
            else if (key >= Key.A && key <= Key.Z)
            {
                string text = key.ToString();
                if (!Shift && !CapsLock)
                    text = text.ToLower();
                return text;
            }
            else if (key == Key.KeypadAdd || key == Key.KeypadPlus)
                return "+";
            else if (key == Key.KeypadSubtract || key == Key.KeypadSubtract)
                return "-";
           else
            {
                switch(key)
                {
                    case Key.Space:
                        return " ";
                    case Key.Tab:
                        return "\t";

                    case Key.Slash:
                        if (Shift)
                            return "?";
                        return "/";

                    case Key.BackSlash:
                        if (Shift)
                            return "|";
                        return "\\";

                    case Key.Comma:
                        if (Shift)
                            return "<";
                        return ",";

                    case Key.Tilde:
                        if (Shift)
                            return "~";
                        return "`";

                    case Key.Plus:
                        if (Shift)
                            return "+";
                        return "=";

                    case Key.Minus:
                        if (Shift)
                            return "_";
                        return "-";
                    
                    case Key.BracketLeft:
                        if (Shift)
                            return "{";
                        return "[";
                    case Key.BracketRight:
                        if (Shift)
                            return "}";
                        return "]";

                    case Key.Semicolon:
                        if (Shift)
                            return ":";
                        return ";";

                    case Key.Quote:
                        if (Shift)
                            return "\"";
                        return "'";

                    case Key.Period:
                        if (Shift)
                            return ">";
                        return ".";

                    case Key.KeypadDivide:
                        return "/";
                    case Key.KeypadMultiply:
                        return "*";
                    case Key.KeypadDecimal:
                        return ".";
                    case Key.Number0:
                        return ")";
                    case Key.Number1:
                        return "!";
                    case Key.Number2:
                        return "@";
                    case Key.Number3:
                        return "#";
                    case Key.Number4:
                        return "$";
                    case Key.Number5:
                        return "%";
                    case Key.Number6:
                        return "^";
                    case Key.Number7:
                        return "&";
                    case Key.Number8:
                        return "*";
                    case Key.Number9:
                        return "(";
                }
            }
            return string.Empty;
        }

        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.CapsLock)
                CapsLock = !CapsLock;

            if (!TextMode)
                Keys.Add(e.Key);
            else
            {
                if (e.Key == Key.ShiftRight || e.Key == Key.ShiftLeft)
                    Shift = true;
                else if (e.Key == Key.Enter || e.Key == Key.KeypadEnter)
                {
                    if (currentText != string.Empty)
                    {
                        lock (currentText)
                        {
                            if (TextModeNewline != null)
                                TextModeNewline(this, e.Key);
                            else
                                currentText += "\n";
                        }
                    }
                }
                else if (e.Key == Key.Delete || e.Key == Key.BackSpace)
                {
                    lock (currentText)
                    {
                        if (currentText.Length > 0)
                            currentText = currentText.Remove(currentText.Length - 1);

                        if (deleteTimer != null)
                            deleteTimer.Dispose();

                        deleteTimer = new Timer(DeleteTimerCallback, null, DeleteRepeatDelay, DeleteRepeatTime);                        
                    }
                }
                else
                {
                    lock (currentText)
                    {
                        currentText += toKey(e.Key);
                    }
                }
            }
        }

        void DeleteTimerCallback(Object state)
        {
            lock (currentText)
            {
                if (currentText.Length > 0)
                    currentText = currentText.Remove(currentText.Length - 1);
            }
        }

        void Keyboard_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (!TextMode && FlushKeysOnUp)
            {
                if (Keys.Contains(e.Key))
                    Keys.Remove(e.Key);
            }

            if (TextMode)
            {
                if (e.Key == Key.ShiftRight || e.Key == Key.ShiftLeft)
                    Shift = false;
                else if (e.Key == Key.Delete || e.Key == Key.BackSpace)
                {
                    deleteTimer.Dispose();
                    deleteTimer = null;
                }
            }
        }
    }
}

/*
    Open Combat/Projekt 2501
    Copyright (C) 2010  Jeffery Allen Myers

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PortalEdit
{
    public class ImageCheckPanel : Panel
    {
        public class ItemCheckChangedEventArgs : EventArgs
        {
            public ItemCheckChangedEventArgs(Button b, bool c)
                : base()
            {
                button = b;
                value = c;
            }

            public bool value;
            public Button button;
        }
        public delegate void ItemCheckChangedEvent(object sender, ItemCheckChangedEventArgs e);
        public event ItemCheckChangedEvent ItemCheckChanged;

        public Color HighlightColor
        {
            get { return highlightColor; }
            set { highlightColor = value; }
        }

        public Color HighlightBGColor
        {
            get { return highlightBGColor; }
            set { highlightBGColor = value; }
        }

        public Button SelectedItem
        {
            get { return selectedItem; }
            set { button_Click(value, EventArgs.Empty); }
        }

        protected Color highlightBGColor = Color.CadetBlue;
        protected Color highlightColor = Color.BlueViolet;

        protected Button selectedItem;
        protected Color BackgroundColor;
        protected Color ForegroundColor;

        protected Dictionary<Button, bool> ButtonStatus = new Dictionary<Button, bool>();

        public ImageCheckPanel()
        {
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (e.Control.GetType() == typeof(Button))
            {
                Button button = (Button)e.Control;
                if (BackgroundColor == null)
                    BackgroundColor = button.BackColor;

                if (ForegroundColor == null)
                    ForegroundColor = button.ForeColor;

                ButtonStatus.Add(button, false);
                button.Click += new EventHandler(button_Click);
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            if (sender.GetType() == typeof(Button) && ButtonStatus.ContainsKey((Button)sender))
            {
                Button button = (Button)sender;
                ButtonStatus[button] = !ButtonStatus[button];

                UpdateButton(button);

                if (ItemCheckChanged != null)
                    ItemCheckChanged(button, new ItemCheckChangedEventArgs(button, ButtonStatus[button]));
            }
        }

        public void CheckButton ( Button button, bool value)
        {
            if (ButtonStatus.ContainsKey(button))
            {
                ButtonStatus[button] = value;
                UpdateButton(button);
                if (ItemCheckChanged != null)
                    ItemCheckChanged(button, new ItemCheckChangedEventArgs(button, ButtonStatus[button]));
            }
        }

        protected void UpdateButton ( Button button )
        {
            if (ButtonStatus.ContainsKey(button))
            {
                if (ButtonStatus[button])
                {
                    button.BackColor = highlightBGColor;
                    button.ForeColor = highlightColor;
                }
                else
                {
                    button.BackColor = BackgroundColor;
                    button.ForeColor = ForegroundColor;
                }
            }
        }
    }
}

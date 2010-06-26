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
    public class ImageRadioPanel : Panel
    {
        public class SelectionChangedEventArgs : EventArgs
        {
            public SelectionChangedEventArgs ( object v, Button b) : base()
            {
                value = v;
                button = b;
            }

            public object value;
            public Button button;
        }
        public delegate void SelectionChangedEvent(object sender, SelectionChangedEventArgs e);
        public event SelectionChangedEvent SelectionChanged;

        public bool TagsAreValues { get; set; }

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

        protected Color highlightBGColor = Color.DarkGoldenrod;
        protected Color highlightColor = Color.BlueViolet;
        protected Button selectedItem;
        protected Color BackgroundColor;
        protected Color ForegroundColor;

        protected Dictionary<Button, object> ButtonTags = new Dictionary<Button, object>();

        public ImageRadioPanel() : base()
        {
            TagsAreValues = false;
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

                button.Click += new EventHandler(button_Click);
            }
        }

        public void SetButtonValue ( Button button, object value )
        {
            ButtonTags.Add(button, value);
        }
        
        protected void ResetButtons ( Button ignore )
        {
            foreach (Control ctl in Controls)
            {
                if (ctl.GetType() == typeof(Button) && ignore != ctl)
                {
                    ctl.BackColor = BackgroundColor;
                    ctl.ForeColor = ForegroundColor;
                }
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            selectedItem = (Button)sender;
            ResetButtons(selectedItem);
            if (selectedItem != null)
            {
                selectedItem.BackColor = highlightBGColor;
                selectedItem.ForeColor = highlightColor;
            }

            if (SelectionChanged != null)
            {
                object tag = null;
                if (TagsAreValues && selectedItem != null)
                    tag = selectedItem.Tag;
                else if ( ButtonTags.ContainsKey(selectedItem))
                    tag = ButtonTags[selectedItem];

                SelectionChanged(this, new SelectionChangedEventArgs(tag, selectedItem));
            }
        }
    }
}

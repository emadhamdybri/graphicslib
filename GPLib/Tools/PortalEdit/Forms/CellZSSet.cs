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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using World;

namespace PortalEdit
{
    public partial class CellZSSet : Form
    {
        public bool Roof = false;
        public bool PresetOnly = false;

        public string SelectedPreset = string.Empty;

        public CellZSSet()
        {
            InitializeComponent();

            PresetList.Items.Add("none");

            PortalMapAttribute[] att = Editor.instance.map.MapAttributes.Find("Editor:NamedDepthSet");
            foreach (PortalMapAttribute at in att)
            {
                string[] nugs = at.Value.Split(":".ToCharArray());
                if (nugs.Length < 3)
                    continue;

                PresetList.Items.Add(nugs[0]);
            }
            PresetList.SelectedIndex = 0;

            if (PresetOnly)
            {
                ZLabel.Enabled = false;
                ZValue.Enabled = false;
                ForceDepth.Enabled = false;
            }
        }

        private void PresetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PresetList.SelectedIndex > 0 && !PresetOnly)
            {
                PortalMapAttribute att = EditFrame.FindDepthAttribute(PresetList.SelectedItem.ToString());
                if (att != null)
                {
                    string[] nugs = att.Value.Split(":".ToCharArray());
                    if (nugs.Length < 3)
                        return;

                    if (nugs[0] == PresetList.SelectedItem.ToString())
                    {
                        if (!Roof)
                            ZValue.Value = decimal.Parse(nugs[1]);
                        else
                            ZValue.Value = decimal.Parse(nugs[2]);
                    }
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (PresetList.SelectedIndex >0)
                SelectedPreset = PresetList.SelectedItem.ToString();
        }
    }
}

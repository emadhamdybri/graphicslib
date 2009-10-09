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

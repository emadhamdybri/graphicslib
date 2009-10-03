using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PortalEdit
{
    public partial class MapImageSetup : Form
    {
        public MapImageSetup()
        {
            InitializeComponent();

            PixelsPerUnit.Value = 100;

            PortalMap map = Editor.instance.map;
            
            PortalMapAttribute[] att = map.FindAttributes("Editor:Image:Underlay:File");
            if (att.Length > 0)
                ImageFileName.Text = att[0].Value;

            att = map.FindAttributes("Editor:Image:Underlay:Scale");
            if (att.Length > 0)
            {
                try
                {
                    decimal val = 0;
                    decimal.TryParse(att[0].Value,out val);
                    PixelsPerUnit.Value = val;
                }
                catch (System.Exception ex)
                {
                }
            }

            att = map.FindAttributes("Editor:Image:Underlay:Offset::X");
            if (att.Length > 0)
            {
                try
                {
                    decimal val = 0;
                    decimal.TryParse(att[0].Value, out val);
                    CenterX.Value = val;
                }
                catch (System.Exception ex)
                {
                }
            }

            att = map.FindAttributes("Editor:Image:Underlay:Offset::Y");
            if (att.Length > 0)
            {
                try
                {
                    decimal val = 0;
                    decimal.TryParse(att[0].Value, out val);
                    CenterY.Value = val;
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        private void ImageFileName_TextChanged(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(ImageFileName.Text);
            if (file.Exists)
            {
                Image image = Image.FromFile(file.FullName);
                if (image != null)
                    ImageInfo.Text = "Image Size X:" + image.Size.Width.ToString() + " Y:" + image.Size.Height.ToString();
            }
        }

        private void BrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open File";
            ofd.Filter = "Portable Network Graphics File (*.png)|*.png|Joint Photographic Experts Group File (*.jpeg)|*.jpeg|Windows Bitmap File (*.bmp)|*.bmp|All Files (*.*)|*.*";

            if (ImageFileName.Text != string.Empty)
            {
                ofd.FileName = ImageFileName.Text;
                ofd.InitialDirectory = Path.GetDirectoryName(ImageFileName.Text);
            }
            else
                ofd.FileName = "*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ImageFileName.Text = ofd.FileName;
            }

        }

        private void OK_Click(object sender, EventArgs e)
        {
            PortalMap map = Editor.instance.map;

            map.RemoveAttributes("Editor:Image:Underlay:File");
            map.RemoveAttributes("Editor:Image:Underlay:Scale");
            map.RemoveAttributes("Editor:Image:Underlay:Offset:X");
            map.RemoveAttributes("Editor:Image:Underlay:Offset:Y");

            if (ImageFileName.Text != String.Empty)
            {
                FileInfo file = new FileInfo(ImageFileName.Text);
                if (file.Exists)
                {
                    map.AddAttribute("Editor:Image:Underlay:File", ImageFileName.Text);
                    map.AddAttribute("Editor:Image:Underlay:Scale", PixelsPerUnit.Value.ToString());
                    map.AddAttribute("Editor:Image:Underlay:Offset:X", CenterX.Value.ToString());
                    map.AddAttribute("Editor:Image:Underlay:Offset:Y", CenterY.Value.ToString());
                    Editor.SetDirty();
                }
                else
                {
                    MessageBox.Show("Image file does not exist");
                    DialogResult = DialogResult.None;
                }
            }
        }
    }
}

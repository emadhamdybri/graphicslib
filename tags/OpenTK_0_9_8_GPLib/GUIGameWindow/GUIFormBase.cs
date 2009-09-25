using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUIGameWindow
{
    public partial class GUIFormBase : Form
    {
        protected GUIGameWindowBase baseWindow;

        public GUIFormBase()
        {
            InitializeComponent();
        }

        public Control GetRootPannel( GUIGameWindowBase window )
        {
            baseWindow = window;
            return this.GUIRootPanel;
        }
    }
}

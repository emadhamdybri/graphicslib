using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;

using GameObjects;

namespace KingsReign
{
    public partial class GameWindow : Form
    {
        public GameClient client;
        public GameVisual visual;

        public GameWindow()
        {
            InitializeComponent();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GameWindow_Load(object sender, EventArgs e)
        {
            visual = new GameVisual(WorldViewCtl);
        }

        private void findGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameConnectionDialog dlog = new GameConnectionDialog();
            if (dlog.ShowDialog(this) == DialogResult.OK)
            {
                GameTimer.Start();
                findGameToolStripMenuItem.Enabled = false;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            client = new GameClient();
        }
    }
}

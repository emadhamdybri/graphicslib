﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.IO;

using GameObjects;

using Utilities.Paths;

namespace KingsReign
{
    public partial class GameWindow : Form
    {
        public GameClient client;
        public GameVisual visual;

        protected Point lastMousePos = new Point(0, 0);

        public GameWindow()
        {
            InitializeComponent();

            ResourceManager.AddPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "data"));
            ResourceManager.AddPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "../data"));
            ResourceManager.AddPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "../../data"));
            ResourceManager.AddPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "../../../data"));
            ResourceManager.AddPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "../../../../data"));
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GameWindow_Load(object sender, EventArgs e)
        {
            visual = new GameVisual(WorldViewCtl);
            GameTimer.Start();

            WorldViewCtl.MouseDown += new MouseEventHandler(WorldViewCtl_MouseDown);
            WorldViewCtl.MouseUp += new MouseEventHandler(WorldViewCtl_MouseUp);
            WorldViewCtl.MouseMove += new MouseEventHandler(WorldViewCtl_MouseMove);

            // automatically fire up a test game
            findGameToolStripMenuItem.Enabled = false;
            client = new GameClient();
            visual.SetClient(client);
            client.InitHosted("map1");

            client.SetupTestPlayer();

            visual.CameraPos = client.State.Players[0].Castles[0].Location;
        }

        void WorldViewCtl_MouseUp(object sender, MouseEventArgs e)
        {
        }

        void WorldViewCtl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                visual.CameraPos.X -= e.X - lastMousePos.X;
                visual.CameraPos.Y -= e.Y - lastMousePos.Y;
            }
            lastMousePos = new Point(e.X, e.Y);

        }

        void WorldViewCtl_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                Point p = new Point(e.X - WorldViewCtl.Width / 2, e.Y - WorldViewCtl.Height / 2);
                visual.SelectPoint = new Point(visual.CameraPos.X + p.X, visual.CameraPos.Y + p.Y);

                if (client != null)
                {
                    visual.MouseClick(visual.SelectPoint);
                    EventLog.Text += "Click at " + visual.SelectPoint.ToString() + " is " + client.State.WorldMap.GetTerrain(visual.SelectPoint).ToString() + "\r\n";

                    EventLog.Select(EventLog.Text.Length-1, EventLog.Text.Length);
                    EventLog.ScrollToCaret();
                }
            }
           
            lastMousePos = new Point(e.X, e.Y);
        }

        private void findGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameConnectionDialog dlog = new GameConnectionDialog();
            if (dlog.ShowDialog(this) == DialogResult.OK)
            {
                findGameToolStripMenuItem.Enabled = false;
                client = new GameClient();
                visual.SetClient(client);
                if (dlog.TestGame)
                    client.InitHosted("map1");
                else
                    client.InitClient("somehost", 1111); // TODO replace with lobby data
            }
        }

        private void Stop ()
        {
            findGameToolStripMenuItem.Enabled = true;
            if (client != null)
                client.Kill();

            client = null;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
           if (client != null)
           {
               if (!client.Update())
                   Stop();
           }
           else if (visual != null)
               visual.IdleUpdate();
        }
    }
}

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

using GUIGameWindow;

#pragma warning disable 612, 618

namespace portalTest
{
    public partial class GameWindow : GUIGameWindowBase
    {
        Game game;

        public GameWindow() : base(1024,768)
        {
            InitializeComponent();

            Mouse.Move += new MouseMoveEventHandler(Mouse_Move);
        }

        void Mouse_Move(MouseDevice sender, MouseMoveEventArgs e)
        {
            if (game != null)
                game.MouseMove(e);
        }

        protected override void SetupGL()
        {
            base.SetupGL();
            game = new Game(this);
            game.visual.SetupGL();          
        }

        protected override void InitGame()
        {
            game.Init();
            Cursor.Position = new Point(Location.X + (Width / 2), Location.X + (Height / 2));
        }

        protected override void Resized(EventArgs e)
        {
            if (game != null)
             game.visual.Resized(e);
        }

        public override void OnUpdateFrame(UpdateFrameArgs e)
        {
            base.OnUpdateFrame(e);
            if (game == null)
                return;
            if (game.Update(e))
                Exit();
        }

        public override void OnRenderFrame(RenderFrameArgs e)
        {
            if (game != null)
                game.visual.RenderFrame();
        }
    }
}

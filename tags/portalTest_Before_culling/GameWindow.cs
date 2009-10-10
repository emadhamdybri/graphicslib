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

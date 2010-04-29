using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace netwar
{
    class Game
    {
        protected Visual visual;

        protected void Init ()
        {
            visual = new Visual(1024,800,OpenTK.Graphics.GraphicsMode.Default,OpenTK.GameWindowFlags.Default);
            visual.Update += new Visual.UpdateEventHandler(visual_Update);
        }

        void visual_Update(Visual sender, double time)
        {
           
        }

        protected void Cleanup()
        {

        }

        public bool Play ()
        {
            Init();

            visual.Run();

            Cleanup();
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using GUIGameWindow;
using World;
using OpenTK;

namespace portalTest
{
    public class Game
    {
        PortalWorld world = null;
        PortalWorldRenderer renderer;

        public Visual visual;

        GUIGameWindowBase window;

        Point lastMousePos = Point.Empty;
        Point thisMousePos = new Point(0, 0);


        public Game(GUIGameWindowBase win)
        {
            window = win;
            world = new PortalWorld();
            renderer = new PortalWorldRenderer(world);
            visual = new Visual(window, renderer);
        }

        public void Init ()
        {
        }

        public void MouseMove ( MouseMoveEventArgs e)
        {
            if (lastMousePos == Point.Empty)
                lastMousePos = new Point(e.X, e.Y);

            thisMousePos = new Point(thisMousePos.X + e.XDelta, thisMousePos.Y + e.YDelta);
        }

        protected bool doInput(GUIGameWindowBase.UpdateFrameArgs e)
        {
            if (window.Keyboard[Keys.Escape])
                return true;

            float turnSpeed = 40.0f;
            turnSpeed *= (float)e.TimeDelta;

            Point delta = thisMousePos;
            thisMousePos = new Point(0, 0);

            float sensitivity = 0.1f;

            visual.camera.turn(-turnSpeed * sensitivity * delta.Y, -turnSpeed * sensitivity * delta.X);

            Vector3 forward = new Vector3(visual.camera.Heading());
            Vector3 leftward = new Vector3(forward);
            leftward.X = -forward.Y;
            leftward.Y = forward.X;

            Vector2 movement = new Vector2();

            float speed = 15.0f;
            speed *= (float)e.TimeDelta;

            if (window.Keyboard[Keys.A])
                movement.X = 1;
            if (window.Keyboard[Keys.D])
                movement.X = -1;
            if (window.Keyboard[Keys.W])
                movement.Y = 1;
            if (window.Keyboard[Keys.S])
                movement.Y = -1;

            if (window.Keyboard[Keys.PageUp])
                visual.camera.move(0, 0, speed);
            if (window.Keyboard[Keys.PageDown])
                visual.camera.move(0, 0, -speed);

            Vector3 incremnt = new Vector3();
            incremnt += forward * movement.Y * speed;
            incremnt += leftward * movement.X * speed;

            visual.camera.move(incremnt);
            return false;
        }

        public bool Update(GUIGameWindowBase.UpdateFrameArgs e)
        {
            return doInput(e);
        }
    }
}

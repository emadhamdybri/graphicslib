using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace GUIGameWindow
{
    public struct MouseMoveEventArgs
    {
        //
        // Summary:
        //     Initializes a new instance of the OpenTK.Input.MouseMoveEventArgs class.
        //
        // Parameters:
        //   x:
        //     The X position.
        //
        //   y:
        //     The Y position.
        //
        //   xDelta:
        //     The change in X position produced by this event.
        //
        //   yDelta:
        //     The change in Y position produced by this event.
        public MouseMoveEventArgs(int x, int y, int xDelta, int yDelta)
        {
            Position = new Point(x, y);
            X = x;
            Y = y;
            XDelta = xDelta;
            YDelta = yDelta;
        }

        // Summary:
        //     Gets a System.Drawing.Points representing the location of the mouse for the
        //     event.
        public Point Position;
        //
        // Summary:
        //     Gets the X position of the mouse for the event.
        public int X;
        //
        // Summary:
        //     Gets the change in X position produced by this event.
        public int XDelta;
        //
        // Summary:
        //     Gets the Y position of the mouse for the event.
        public int Y;
        //
        // Summary:
        //     Gets the change in Y position produced by this event.
        public int YDelta;
    } 
    public struct MouseButtonEventArgs
    {
        public MouseButtonEventArgs(MouseButtons b, bool down, Point pos)
        {
            this.Button = b;
            this.IsPressed = down;
            this.Position = pos;
            this.X = pos.X;
            this.Y = pos.Y;
        }
        // Summary:
        //     The mouse button for the event.
        public MouseButtons Button;
        //
        // Summary:
        //     Gets a System.Boolean representing the state of the mouse button for the
        //     event.
        public bool IsPressed;
        //
        // Summary:
        //     Gets a System.Drawing.Points representing the location of the mouse for the
        //     event.
        public Point Position;
        //
        // Summary:
        //     Gets the X position of the mouse for the event.
        public int X;
        //
        // Summary:
        //     Gets the Y position of the mouse for the event.
        public int Y;
    }
    public struct MouseWheelEventArgs
    {
        //
        // Summary:
        //     Initializes a new instance of the OpenTK.Input.MouseWheelEventArgs class.
        //
        // Parameters:
        //   x:
        //     The X position.
        //
        //   y:
        //     The Y position.
        //
        //   value:
        //     The value of the wheel.
        //
        //   delta:
        //     The change in value of the wheel for this event.
        public MouseWheelEventArgs(int x, int y, int value, int delta)
        {
            Delta = delta;
            Position = new Point(x, y);
            Value = value;
            X = x;
            Y = y;
        }

        // Summary:
        //     The change in value of the wheel for this event.
        public int Delta;
        //
        // Summary:
        //     Gets a System.Drawing.Points representing the location of the mouse for the
        //     event.
        public Point Position;
        //
        // Summary:
        //     The value of the wheel.
        public int Value;
        //
        // Summary:
        //     Gets the X position of the mouse for the event.
        public int X;
        //
        // Summary:
        //     Gets the Y position of the mouse for the event.
        public int Y;
    }
   
    public delegate void MouseMoveEventHandler(MouseDevice sender, MouseMoveEventArgs e);
    public delegate void MouseButtonEventHandler(object sender, MouseButtonEventArgs e);
    public delegate void MouseWheelEventHandler(object sender, MouseWheelEventArgs e);

    public class MouseDevice
    {
        public event MouseButtonEventHandler ButtonDown;
        public event MouseButtonEventHandler ButtonUp;
        public event MouseMoveEventHandler Move;
        public event MouseWheelEventHandler WheelChanged;

        Dictionary<MouseButtons, bool> buttonMap = new Dictionary<MouseButtons,bool>();

        public bool this[MouseButtons button]
        {
            get 
            {
                if (!buttonMap.ContainsKey(button))
                    return false;
                else
                    return buttonMap[button]; 
            }
        }

        public int X
        {
            get { return lastPoint.X; }
        }
        public int Y
        {
            get { return lastPoint.Y; }
        }
        public int XDelta
        {
            get { return lastDelta.X; }
        }
        public int YDelta
        {
            get { return lastDelta.Y; }
        }

        public int NumberOfButtons
        {
            get { return 5; }
        }

        public int NumberOfWheels
        {
            get { return 1; }
        }

        Point lastPoint = new Point(0, 0);
        Point lastDelta = new Point(0, 0);
        bool lastPointSet = false;

        int wheelCount = 0;
        int lastWheelDelta = 0;

        public MouseDevice(Control ctl)
        {
            ctl.MouseDown += new MouseEventHandler(InternalDown);
            ctl.MouseUp += new MouseEventHandler(InternalUp);

            ctl.MouseMove += new MouseEventHandler(InternalMove);
            ctl.MouseWheel += new MouseEventHandler(InternalWheel);
        }

        protected void InternalDown(Object sender, MouseEventArgs args)
        {
            buttonMap[args.Button] = true;
            if (ButtonDown != null)
                ButtonDown(this, new MouseButtonEventArgs(args.Button, true, args.Location));
        }

        protected void InternalUp(Object sender, MouseEventArgs args)
        {
            buttonMap[args.Button] = false;
            if (ButtonUp != null)
                ButtonUp(this, new MouseButtonEventArgs(args.Button, false, args.Location));
        }

        protected void InternalMove(Object sender, MouseEventArgs args)
        {
            if (lastPointSet)
            {
                lastDelta = new Point(args.Location.X - lastPoint.X,args.Location.Y - lastPoint.Y);
                if (Move != null)
                    Move(this, new MouseMoveEventArgs(args.Location.X, args.Location.Y, lastDelta.X, lastDelta.Y));
            }

            lastPoint = new Point(args.Location.X,args.Location.Y);
            lastPointSet = true;
        }

        protected void InternalWheel(Object sender, MouseEventArgs args)
        {
            lastWheelDelta = args.Delta;
            wheelCount += args.Delta;

            lastDelta = new Point(args.Location.X - lastPoint.X, args.Location.Y - lastPoint.Y);
            lastPoint = new Point(args.Location.X, args.Location.Y);
            lastPointSet = true;
            
            if (WheelChanged != null)
                WheelChanged(this,new MouseWheelEventArgs(args.Location.X,args.Location.Y,wheelCount,args.Delta));
        }
    }
}

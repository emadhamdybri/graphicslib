using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using OpenTK;

namespace Simulation
{
    public class ObjectState
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Heading = Vector3.Zero;
       
        public Vector3 Movement = Vector3.Zero;
        public Vector3 Spin = Vector3.Zero;
    }

    public class SimObject : Object
    {
        public Object Tag = null;
        
        public UInt64 ID;

        public float Radius = 0f;

        public ObjectState CurrentState = new ObjectState();

        public ObjectState LastUpdateState = new ObjectState();
        public double LastUpdateTime = -1;

        protected Vector3 PredictPosition(float delta)
        {
            return LastUpdateState.Position + LastUpdateState.Movement * delta;
        }

        protected Vector3 PredictHeading(float delta)
        {
            return LastUpdateState.Heading + LastUpdateState.Spin * delta;
        }

        public virtual void Update(double time)
        {
            float delta = (float)(time - LastUpdateTime);

            CurrentState.Position = PredictPosition(delta);
            CurrentState.Heading = PredictHeading(delta);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Simulation
{
    public class Shot : SimObject
    {
        public Player Owner;
        public float Damage = 0;
        public double Lifetime = 10.0;

        ObjectState lastBounce = new ObjectState();
        double lastBounceTime = 0;

        public virtual void Update(double time)
        {
            float delta = (float)(time - LastUpdateTime);
            Vector3 pos = PredictPosition(delta);
            Vector3 heading = PredictHeading(delta);

            // do bounce collisions

            CurrentState.Position = pos;
            CurrentState.Heading = heading;
        }

        public bool Expired ( double time )
        {
            return (time - LastUpdateTime) > Lifetime;
        }
    }
}

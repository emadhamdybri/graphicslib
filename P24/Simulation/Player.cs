﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Simulation
{
    public class Player : SimObject
    {
        public virtual void Update(double time)
        {
            float delta = (float)(time - LastUpdateTime);
            Vector3 pos = PredictPosition(delta);
            Vector3 heading = PredictHeading(delta);

            // do bounce collisions

            CurrentState.Position = pos;
            CurrentState.Heading = heading;
        }
    }
}
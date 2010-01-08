using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Math3D;

namespace Simulation
{
    public enum PlayerStatus
    {
        Connecting = 0,
        Despawned = 1,
        Alive = 2,
        Dead = 3,
    }

    public class Player : SimObject
    {
        public string Callsign = string.Empty;
        public int Score = -1;
        public string Pilot = string.Empty;

        float intendedAngle = 0;
        Vector2 intendedSpeed = new Vector2(0,0);

        public PlayerStatus Status = PlayerStatus.Connecting;

        bool hasInput = false;

        public virtual float TurningSpeed
        {
            get { return sim.Settings.BaseTurnSpeed; }
        }

        public virtual float TurningAcceleration
        {
            get { return sim.Settings.BaseTurnAcceleration; }
        }

        public virtual float ForwardSpeed
        {
            get { return sim.Settings.BaseSpeed; }
        }

        public virtual float BackwardSpeed
        {
            get { return sim.Settings.BaseSpeed * 0.5f; }
        }

        public virtual float SidewaysSpeed
        {
            get { return sim.Settings.BaseSpeed * 0.25f; }
        }

        internal Player ( Sim s ) :base(s)
        {

        }

        public virtual void CopyFrom ( Player player )
        {
            base.CopyFrom(player);
            Callsign = player.Callsign;
            Score = player.Score;
            Pilot = player.Pilot;
            Status = player.Status;
        }

        protected virtual void ProcessInput(double time)
        {
            if (!hasInput)
                return;

            double timeDelta = time - LastUpdateTime;

            // rotation
            // check the intended angle and see if we are close to being there (we will be this update)
            // if so just snap to it.
            float delta = CurrentState.Rotation - intendedAngle;
            if (Math.Abs(delta) < LastUpdateState.Spin * timeDelta)
            {
                CurrentState.Rotation = LastUpdateState.Rotation = intendedAngle;
                CurrentState.Spin = LastUpdateState.Spin = 0;
                LastUpdateTime = time;
            }
            else 
            {
                if (Math.Abs(LastUpdateState.Spin) < TurningSpeed)// see if we are turning at max speed, if not go faster
                    LastUpdateState.Spin += TurningAcceleration * (float)timeDelta;
                else if (Math.Abs(LastUpdateState.Spin) > TurningSpeed)
                    LastUpdateState.Spin = TurningSpeed;

                LastUpdateTime = time;
                LastUpdateState.Spin = delta / delta;
            }

            //thrust
            float speed = LastUpdateState.Movement.Length;

            // see if we are going forward or backwards

        }

        public override void Update(double time)
        {
            if (Status == PlayerStatus.Alive)
            {
                ProcessInput(time);

                float delta = (float)(time - LastUpdateTime);
                Vector3 pos = PredictPosition(delta);
                float rot= PredictRotation(delta);

                // do bounce collisions

                CurrentState.Position = pos;
                CurrentState.Rotation = rot;
                CurrentState.Heading = VectorHelper3.FromAngle(rot);
            }
        }

        public virtual void Turn ( float param )
        {
            hasInput = false;
            LastUpdateState.Spin = TurningSpeed * param;
        }

        public virtual void TurnTo ( float angle )
        {
            hasInput = true;
            intendedAngle = angle;
        }

        public virtual void Thrust ( float param )
        {
            hasInput = true;
            if (param > 0)
                intendedSpeed.X = ForwardSpeed * param;
            else
                intendedSpeed.X = BackwardSpeed * param;
        }

        public virtual void Sideswipe ( float param )
        {
            hasInput = true;
            intendedSpeed.Y = SidewaysSpeed * param;
        }
    }
}

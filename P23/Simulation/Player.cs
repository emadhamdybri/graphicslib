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

        public PlayerStatus Status = PlayerStatus.Connecting;

        public virtual float TurningSpeed
        {
            get { return sim.Settings.MaxTurnSpeed; }
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

        public override void Update(double time)
        {
            if (Status == PlayerStatus.Alive)
            {
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
            LastUpdateState.Spin = TurningSpeed * param;
        }
    }
}

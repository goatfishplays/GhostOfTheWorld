using UnityEngine;
using Utilities;

namespace PlatformerAI
{
    public interface IDetectionStrategy
    {
        bool Execute(Transform player, Transform detector, CountdownTimer timer);

    }

    public class ConeDetectionStrategy : IDetectionStrategy
    {
        readonly float detectionAngle;
        readonly float detectionRadious;
        readonly float innerDetectionRadious;

        public ConeDetectionStrategy(float detectionAngle, float detectionRadious, float innerDetectionRadious)
        {
            this.detectionAngle = detectionAngle;
            this.detectionRadious = detectionRadious;
            this.innerDetectionRadious = innerDetectionRadious;
        }

        public bool Execute(Transform player, Transform detector, CountdownTimer timer)
        {
            if (timer.IsRunning) return false;

            var directionToPlayer = player.position - detector.position; ;
            var angleToPlayer = Vector3.Angle(directionToPlayer, detector.forward);

            // if the player is not in the detectiion angle and outer radious 
            //aka the cone in front of the player
            // or outside the inner radious, return false
            if ((!(angleToPlayer < detectionAngle / 2f) || !(directionToPlayer.magnitude < detectionRadious))
                && !(directionToPlayer.magnitude < innerDetectionRadious))
            {
                return false;
            }
            timer.Start();
            return true;
        }
    }



}




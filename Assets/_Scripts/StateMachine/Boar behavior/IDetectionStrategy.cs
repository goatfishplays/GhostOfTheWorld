using System.Runtime.CompilerServices;
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
        readonly float detectionRadius;
        readonly float innerDetectionRadius;

        public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius)
        {
            this.detectionAngle = detectionAngle;
            this.detectionRadius = detectionRadius;
            this.innerDetectionRadius = innerDetectionRadius;
        }

        public bool Execute(Transform player, Transform detector, CountdownTimer timer)
        {
            if (timer.IsRunning) return false;

            Vector3 directionToPlayer = player.position - detector.position;
            float distanceToPlayer = directionToPlayer.magnitude;
            float angleToPlayer = Vector3.Angle(directionToPlayer.normalized, detector.forward);

            bool isInInnerRadius = distanceToPlayer < innerDetectionRadius;
            bool isInCone = angleToPlayer < detectionAngle / 2f && distanceToPlayer < detectionRadius;

            if (!(isInInnerRadius || isInCone))
                return false;

            // Check for line of sight
            Vector3 eyeLevel = detector.position + Vector3.up * 1f; // Adjust height if needed
            Vector3 shootDir = (player.position - eyeLevel).normalized;
            LayerMask detectionMask = LayerMask.GetMask("Default", "Player");

            RaycastHit hit;
            if (Physics.Raycast(eyeLevel, shootDir, out hit, distanceToPlayer,detectionMask))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    return false; // Something is blocking the view
                }
            }
            else
            {
                return false; // Nothing was hit — very unlikely, but a safe fallback
            }

            timer.Start();
            return true;
        }

    }



}




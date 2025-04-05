using UnityEngine;
using Utilities;


namespace PlatformerAI
{
    public class PlayerDectector : MonoBehaviour
    {
        [SerializeField] float detectionAngle = 60f; // cone in front of enemy
        [SerializeField] float detectionRadious = 10f; // enemy vision aka Large circle around enemy
        [SerializeField] float innerDetectionRadious = 5f; // for checking if the player reach too clos from behind
        [SerializeField] float detectionCooldown = 1f; // cooldown
        //[SerializeField] float attackRange = 10f;

        public Transform GetPlayer() => Player;
        public Transform Player { get; private set; }
        CountdownTimer detectionTimer;

        IDetectionStrategy detectionStrategy;

        void Start()
        {
            detectionTimer = new CountdownTimer(detectionCooldown);
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadious, innerDetectionRadious);

        }

        void Update() => detectionTimer.Tick(Time.deltaTime);
        public bool canDetectPlayer()
        {
            return detectionTimer.IsRunning || detectionStrategy.Execute(Player, transform, detectionTimer);
        }

        /*public bool canAttack()
        {
            var directionToPlayer = Player.position - transform.position;
            return directionToPlayer.magnitude <= attackRange;
        }*/


        // make sure that whenever you cretae a new odj, it will hold the code by call new
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;
        

    }

}


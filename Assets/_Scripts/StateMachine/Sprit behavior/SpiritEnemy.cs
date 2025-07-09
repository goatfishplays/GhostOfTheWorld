using UnityEngine;
using Utilities;



namespace PlatformerAI
{
    [RequireComponent(typeof(ProjectileSpawner))]
    public class SpiritEnemy : AttackEnemy
    {
        [SerializeField] protected SpiritSO spiritSO = null;
        [SerializeField] private ProjectileSpawner PS;

        
        protected override void Start()
        {
            attackEnemySO = spiritSO;
            attackState = new EnemyAttackStateSpirit(this, agent, PlayerDectector, spiritSO);
            
            // Run base Start function to prepare State machine.
            base.Start();

            // Attack timer isn't used for spirit. Probably can remove
            // attackTimer = new CountdownTimer(attackCooldown);

            


            StateMachine.SetState(wanderState);

        }
        public override void Attack(Entity target)
        {
            /*if (attackTimer.IsRunning) return;
            attackTimer.Start();*/
            if (target == null) return;
            Debug.Log("Spirit enemy shoots");

            // TODO: put this into the attack state.
            // Face the player before shooting
            Vector3 lookDirection = (target.transform.position - transform.position).normalized;
            lookDirection.y = 0;
            transform.forward = lookDirection;

            PS.Shoot();
            //attackTimer.Start();

            /*Vector3 shootDirection = (target.transform.position - firePoint.position).normalized;
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            var projectile = bullet.GetComponent<BulletLogic>();
            projectile.Initialize(shootDirection, entity);*/
        }

        public override void Jump(Entity target)
        {
            throw new System.NotImplementedException();
        }
    }
}

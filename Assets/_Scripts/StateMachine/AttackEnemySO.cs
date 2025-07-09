using UnityEngine;

public class AttackEnemySO : ScriptableObject
{
    [Header("Attack Enemy Stats")]
    [Tooltip("Max distance they will randomly wander to from their current location. Basically how far they move each movement when wandering")]
    public float wanderRadius = 5f;
    [Tooltip("Time between attacks")]
    public float attackCooldown = 2f;
    [Tooltip("Distance before the enemy starts doing attacks.")]
    public float attackRange = 10f;
}

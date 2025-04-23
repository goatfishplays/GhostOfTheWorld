using PlatformerAI;
using UnityEngine.AI;
using UnityEngine;
using Utilities;


public abstract class BaseEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public PlayerDectector PlayerDectector;
    public CountdownTimer attackTimer;
    

    public abstract void Attack(Entity target);
    public abstract void Jump(Entity target);

    
}
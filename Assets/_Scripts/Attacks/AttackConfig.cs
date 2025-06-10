using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AttackConfig")]
public class AttackConfig : ScriptableObject
{
    [Range (0f, 300f)]
    [Tooltip("Amount of time in seconds before the attack is destroyed automatically")]
    public float attackLifetime = 100f;
    [Range (0f, 100f)]
    public float damage = 10f;
    [Range (0f, 1f)]
    public float iFramesAddTime = 0.1f;
    public bool ignoresIFrames = false;
    public bool destroyOnHit = true;
}

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AttackConfig")]
public class AttackConfig : ScriptableObject
{
    [Range (0f, 1000f)]
    public float attackLifetime = 1000f;
    [Range (0f, 100f)]
    public float damage = 1f;
    [Range (0f, 1f)]
    public float iFramesAddTime = 0.2f;
    public bool ignoresIFrames = false;
    public bool destroyOnHit = false;
}

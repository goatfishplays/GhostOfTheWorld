using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/WolfSO")]
public class WolfSO : AttackEnemySO
{
    [Header("Wolf Stats")]
    [Tooltip("Min range the wolf can start a jump from.")]
    public float jumpRangeMin = 10f;
    [Tooltip("Max range the wolf can start a jump from. Should be smaller than detection radius.")]
    public float jumpRangeMax = 15f;
    [Tooltip("Number of seconds the wolf takes to complete a jump.")]
    public float jumpTimeLength = 2f;
    [Tooltip("Time in seconds between wolf jumping. Cooldown starts the moment they start jumping.")]
    public float jumpCooldown = 5f;
    [Tooltip("Curve the wolf follows when jumping. Time is adjusted to the jumpTimeLength.")]
    public AnimationCurve heightCurve;
}

using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/BoarSO")]
public class BoarSO : AttackEnemySO
{
    [Header("Boar Stats")]
    public float chargeSpeed = 30f;
    public float chargeAcceleration = 20f;
    public float chargeDistance = 20f;
}

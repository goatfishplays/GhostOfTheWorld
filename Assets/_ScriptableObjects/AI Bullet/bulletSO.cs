using UnityEngine;

[CreateAssetMenu(fileName = "bulletSO", menuName = "Scriptable Objects/bulletSO")]
public class bulletSO : ScriptableObject
{
    
    public float speed = 10f;
    public float damage = 5f;
    public float lifetime = 5f;
}


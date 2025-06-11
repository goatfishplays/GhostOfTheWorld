using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField] private bulletSO bulletData;
    private Vector3 direction;
    private Entity owner;

    public void Initialize(Vector3 shootDirection, Entity shooter)
    {
        direction = shootDirection.normalized;
        owner = shooter;
        Destroy(gameObject, bulletData.lifetime);
    }

    void Update()
    {
        transform.position += direction * bulletData.speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity targetEntity = other.GetComponent<Entity>();

        // Avoid hitting self or friendly fire
        if (targetEntity != null && targetEntity != owner)
        {
            // Optional: Check tags or team to filter who can be damaged
            if (other.CompareTag("Player")) // Make sure your player is tagged correctly
            {
                targetEntity.entityHealth.ChangeHealth(-bulletData.damage);
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public Entity owner;
    public Transform projectileSpawnPoint;
    public Transform viewDirection;
    public GameObject projectilePrefab;
    void Shoot()
    {
        // Creates a projectile and puts it at the player and with the same position and rotation.
        GameObject p = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        Rigidbody rb = p.GetComponent<Rigidbody>();

        rb.linearVelocity = viewDirection.forward;
        p.GetComponent<Attack>().ownerId = owner.id;
    }
}

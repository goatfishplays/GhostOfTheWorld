using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public Entity owner;
    public Transform projectileSpawnPoint;
    public Transform viewDirection;
    public GameObject projectilePrefab;
    [Tooltip("Usually --- Attacks ---")]
    public Transform parent;
    public float projectileSpeed = 10f; // TODO: Move this into the weapon's properties

    public void Shoot()
    {
        // Creates a projectile and puts it at the player and with the same position and rotation.
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation, parent);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.linearVelocity = viewDirection.forward * projectileSpeed;
        projectile.GetComponent<Attack>().ownerId = owner.id;
    }
}

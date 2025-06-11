using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Tooltip("Entity that this spawner is attached to")]
    public Entity owner;
    [Tooltip("Where the projectile will spawn from with it's position and rotation")]
    public Transform projectileSpawnPoint;
    [Tooltip("The direction is taken from this Transform and given to the projectile")]
    public Transform viewDirection;
    [Tooltip("ProjectilePrefab from the Attack Prefab Folder")] 
    public GameObject projectilePrefab;
    [Tooltip("Usually the game object '--- Attacks ---'")]
    public Transform parent;
    public float projectileSpeed = 10f;

    public void Shoot()
    {
        // Creates a projectile and copies the position and rotation from the projectileSpawnPoint.
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation, parent);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Apply linear velocity in the direction of the view direction
        rb.linearVelocity = viewDirection.forward * projectileSpeed;

        projectile.GetComponent<Attack>().ownerId = owner.id;
    }
}

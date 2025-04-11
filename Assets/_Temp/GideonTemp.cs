using UnityEngine;
using UnityEngine.InputSystem;

public class Gideon_Temp : MonoBehaviour
{
    public Entity player;
    public PlayerInput playerInput;
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public Camera mainCam;
    public float projectileSpeed = 4f;

    private InputAction damageAction;
    private InputAction shootAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageAction = playerInput.actions.FindAction("TempDamage");
        damageAction.performed += TakeDamage;

        shootAction = playerInput.actions.FindAction("TempShoot");
        shootAction.performed += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(InputAction.CallbackContext context)
    {
        player.entityHealth.ChangeHealth(-5f);
    }

    void Shoot(InputAction.CallbackContext context)
    {
        // Creates a projectile and puts it at the player and with the same position and rotation.
        GameObject p = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        
        Rigidbody rb = p.GetComponent<Rigidbody>();
        
        rb.linearVelocity = mainCam.transform.forward * projectileSpeed;
        p.GetComponent<Attack>().ownerId = player.id;

        Debug.Log(mainCam.transform.forward);
    }
}

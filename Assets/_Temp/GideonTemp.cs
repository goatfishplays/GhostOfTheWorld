using UnityEngine;
using UnityEngine.InputSystem;

public class Gideon_Temp : MonoBehaviour
{
    public Entity player;
    private InputAction damageAction;
    public PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageAction = playerInput.actions.FindAction("TempDamage");
        damageAction.performed += TakeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage(InputAction.CallbackContext context)
    {
        player.entityHealth.ChangeHealth(-5f);
    }
}

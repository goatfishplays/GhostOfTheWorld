using UnityEngine;
using UnityEngine.InputSystem;

public class Gideon_Temp : MonoBehaviour
{
    public Entity player;
    public PlayerInput playerInput;
    public Camera mainCam;

    private InputAction damageAction;


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
        player.entityHealth.Hit(-5f);
    }
}

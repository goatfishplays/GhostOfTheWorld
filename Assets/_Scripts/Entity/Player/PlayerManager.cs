using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Entity entity;
    public PlayerInput playerInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Two Player Managers Found, Deleting Second");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector2 planarMovementInput = playerInput.actions["move"].ReadValue<Vector2>();
        // Debug.Log(planarMovementInput);
        entity.entityMovement.Move(planarMovementInput);
    }
}

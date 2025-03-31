using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Entity entity;
    public DashController dashController;
    public PlayerInput playerInput;
    private InputAction movementAction;
    private InputAction lookAction;
    [Header("Look")]
    public PlayerCameraControl playerCameraControl;
    private InputAction dashAction;
    [Header("Sprint")]
    [SerializeField] private float sprintMult = 1.75f;
    private InputAction sprintAction;
    private const string SPRINT_SPEED_MULT_ID = "sprint";

    [Header("Inventory")]
    [SerializeField] ItemUIInventoryController inventoryUI;
    private InputAction inventoryAction;
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
        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // get inputs
        movementAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        dashAction = playerInput.actions.FindAction("Dash");
        sprintAction = playerInput.actions.FindAction("Sprint");
        inventoryAction = playerInput.actions.FindAction("Inventory");
        // lookAction.performed += context => { playerCameraControl.AddRotation(context.ReadValue<Vector2>()); };
        lookAction.performed += Look;
        dashAction.started += Dash;
        sprintAction.started += StartSprint;
        sprintAction.canceled += EndSprint;
        inventoryAction.started += ToggleInventory;
    }



    private void OnEnable()
    {
        movementAction.Enable();
        lookAction.Enable();
        dashAction.Enable();
        sprintAction.Enable();
        inventoryAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.Disable();
        lookAction.Disable();
        dashAction.Disable();
        sprintAction.Disable();
        inventoryAction.Disable();
    }

    void OnDestroy()
    {
        movementAction.performed -= Look;
        dashAction.started -= Dash;
        sprintAction.started -= StartSprint;
        sprintAction.canceled -= EndSprint;
        inventoryAction.started -= ToggleInventory;
    }

    // Update is called once per frame 
    void Update()
    {
    }

    void FixedUpdate()
    {
        Vector2 planarMovementInput = movementAction.ReadValue<Vector2>();
        // Debug.Log(planarMovementInput);
        entity.entityMovement.Move(planarMovementInput);
    }

    public void Look(InputAction.CallbackContext context)
    {
        // Debug.Log(context); 
        playerCameraControl.AddRotation(context.ReadValue<Vector2>());
    }

    public void Dash(InputAction.CallbackContext context)
    {
        dashController.AttemptDash(movementAction.ReadValue<Vector2>());
    }

    public void StartSprint(InputAction.CallbackContext context)
    {
        entity.entityMovement.AddTargetSpeedMult(SPRINT_SPEED_MULT_ID, sprintMult);
    }

    public void EndSprint(InputAction.CallbackContext context)
    {
        entity.entityMovement.RemoveTargetSpeedMult(SPRINT_SPEED_MULT_ID);
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeInHierarchy);
        if (inventoryUI.gameObject.activeInHierarchy)
        {
            inventoryUI.OpenInventory();
            Cursor.lockState = CursorLockMode.None;
            lookAction.Disable();
        }
        else
        {
            lookAction.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}

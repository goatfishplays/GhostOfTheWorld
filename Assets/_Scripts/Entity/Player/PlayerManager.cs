using UnityEditor.Build.Player;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Rendering.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Entity entity;
    public DashController dashController;
    public PlayerInput playerInput;

    private InputAction movementAction;

    [Header("Look")]
    public PlayerCameraControl playerCameraControl;
    private InputAction lookAction;
    private InputAction dashAction;

    [Header("Sprint")]
    [SerializeField] private float sprintMult = 1.75f;
    private InputAction sprintAction;
    private const string SPRINT_SPEED_MULT_ID = "sprint";

    [Header("Inventory")]
    // [SerializeField] ItemUIInventoryController inventoryUI; 
    public Inventory inventory;
    private InputAction inventoryAction;

    [Header("Item")]
    [SerializeField] private float itemUseTime = 0f;
    [SerializeField] private bool itemUsed = false;
    [SerializeField] private Image primaryItemFill;
    private const string ITEM_USE_SPEED_MULT_ID = "ItemUse";
    private InputAction itemAction;

    [Header("Interact")]
    [SerializeField] private PlayerInteracter playerInteracter;
    private InputAction interactAction;
    private InputAction shiftAction;

    [Header("Shoot")]
    [SerializeField] private ProjectileSpawner projectileSpawner;
    private InputAction shootAction;

    // [Header("Menus")]
    private InputAction menuAction;
    public MenuManager menuManager => MenuManager.instance;

    // private Coroutine co_itemDelay = null;
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

        if (projectileSpawner == null)
        {
            Debug.LogWarning("There is no projectileSpawner attached to the player");
        }

        entity.entityHealth.OnDie += onPlayerDie;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Get inputs
        movementAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        dashAction = playerInput.actions.FindAction("Dash");
        sprintAction = playerInput.actions.FindAction("Sprint");
        shootAction = playerInput.actions.FindAction("Shoot");
        itemAction = playerInput.actions.FindAction("Item");
        inventoryAction = playerInput.actions.FindAction("Inventory");
        interactAction = playerInput.actions.FindAction("Interact");
        shiftAction = playerInput.actions.FindAction("Shift");
        menuAction = playerInput.actions.FindAction("Menu");

        // Set all of the actions to their corresponding functions.
        // ! Remember to set the OnEnable(), OnDisable(), and OnDestroy()
        // lookAction.performed += context => { playerCameraControl.AddRotation(context.ReadValue<Vector2>()); };
        lookAction.performed += Look;
        dashAction.started += Dash;
        sprintAction.started += StartSprint;
        sprintAction.canceled += EndSprint;
        shootAction.started += Shoot;
        itemAction.performed += ProgressItemUse;
        itemAction.canceled += ItemKeyRelease;
        inventoryAction.started += ToggleInventory;
        interactAction.started += StartInteract;
        interactAction.canceled += EndInteract;
        menuAction.started += ToggleMenu;
    }

    private void OnEnable()
    {
        movementAction.Enable();
        lookAction.Enable();
        dashAction.Enable();
        sprintAction.Enable();
        shootAction.Enable();
        itemAction.Enable();
        inventoryAction.Enable();
        interactAction.Enable();
        shiftAction.Enable();
        menuAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.Disable();
        lookAction.Disable();
        dashAction.Disable();
        sprintAction.Disable();
        shootAction.Disable();
        itemAction.Disable();
        inventoryAction.Disable();
        interactAction.Disable();
        shiftAction.Disable();
        menuAction.Disable();
    }

    void OnDestroy()
    {
        lookAction.performed -= Look;
        dashAction.started -= Dash;
        sprintAction.started -= StartSprint;
        sprintAction.canceled -= EndSprint;
        shootAction.started -= Shoot;
        itemAction.performed -= ProgressItemUse;
        itemAction.canceled -= ItemKeyRelease;
        inventoryAction.started -= ToggleInventory;
        interactAction.started -= StartInteract;
        interactAction.canceled -= EndInteract;
        menuAction.started -= ToggleMenu; 
    }

    // Update is called once per frame 
    void Update()
    {
        if (inventory.CanUsePrimary() && !itemUsed && itemAction.IsInProgress())
        {
            itemUseTime += Time.deltaTime;
            primaryItemFill.fillAmount = itemUseTime / inventory.primaryItem.useTime;
            if (itemUseTime >= inventory.primaryItem.useTime)
            {
                inventory.AttemptUsePrimary();
                // inventory.primaryItem.AttemptUse(entity); 
                itemUsed = true;

                // try to use again if can
                if (inventory.primaryItem.useTime != 0)
                {
                    if (inventory.CanUsePrimary())
                    {
                        ResetItemUse();
                    }
                    else
                    {
                        entity.entityMovement.RemoveTargetSpeedMult(ITEM_USE_SPEED_MULT_ID);
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 planarMovementInput = movementAction.ReadValue<Vector2>();
        // Debug.Log(planarMovementInput);
        entity.entityMovement.Move(planarMovementInput);
    }

    public void SetLookState(bool canLook = true)
    {
        if (canLook)
        {
            lookAction.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            lookAction.Disable();
        }
    }

    public void LockInputs(bool inputsLocked, bool cursorLocked)
    {
        SetLookState(!cursorLocked);
        if (inputsLocked)
        {
            movementAction.Disable();
            dashAction.Disable();
            sprintAction.Disable();
            shootAction.Disable();
            itemAction.Disable();
            interactAction.Disable();
            shiftAction.Disable();
        }
        else
        {
            movementAction.Enable();
            dashAction.Enable();
            sprintAction.Enable();
            shootAction.Enable();
            itemAction.Enable();
            interactAction.Enable();
            shiftAction.Enable();
        }
    }

    public void Look(InputAction.CallbackContext context)
    {
        // Debug.Log(context);
        Vector2 dir = context.ReadValue<Vector2>();
        // Add Sensitivity
        dir.x *= OptionsManager.instance.xSensitivity;
        dir.y *= OptionsManager.instance.ySensitivity;
        playerCameraControl.AddRotation(dir);
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

    public void Shoot(InputAction.CallbackContext context)
    {
        if (projectileSpawner != null)
        {
            projectileSpawner.Shoot();
        }
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        switch (menuManager.state)
        {
            case MenuManager.MenuState.None:
                menuManager.SetState(MenuManager.MenuState.Pause);
                break;
            default:
                menuManager.SetState(MenuManager.MenuState.None);
                break;
        }
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        if (menuManager.state == MenuManager.MenuState.None)
        {
            menuManager.SetState(MenuManager.MenuState.Inventory);
        }
        else if (menuManager.state == MenuManager.MenuState.Inventory)
        {
            menuManager.SetState(MenuManager.MenuState.None);
        }
    }

    public void ItemKeyRelease(InputAction.CallbackContext context)
    {
        entity.entityMovement.RemoveTargetSpeedMult(ITEM_USE_SPEED_MULT_ID);
        ResetItemUse();
    }

    public void ResetItemUse()
    {
        primaryItemFill.fillAmount = 0;

        itemUseTime = 0;
        itemUsed = false;
    }

    public void ProgressItemUse(InputAction.CallbackContext context)
    {
        //     itemUseTime += Time.deltaTime;
        //     Debug.Log(Time.deltaTime); 
        if (inventory.primaryItem.useTime == 0f)
        {
            return;
        }
        entity.entityMovement.AddTargetSpeedMult(ITEM_USE_SPEED_MULT_ID, inventory.primaryItem.useMovementSpeedMult);

    }

    public void StartInteract(InputAction.CallbackContext context)
    {
        playerInteracter.interactionHeld = true;
        playerInteracter.areaToggle = shiftAction.IsPressed();
    }

    public void EndInteract(InputAction.CallbackContext context)
    {
        playerInteracter.interactionHeld = false;
    }

    private void onPlayerDie()
    {
        LockInputs(true, true);
        entity.rb.linearVelocity = Vector3.zero;
    }

    // public void StartItemUse(InputAction.CallbackContext context)
    // {
    //     if (co_itemDelay != null)
    //     {
    //         StopCoroutine(co_itemDelay);
    //     }
    //     Item item = inventory.primaryItem; 
    //     if (item.useTime == 0f)
    //     {
    //         item.AttemptUse(entity);
    //         return;
    //     }
    //     co_itemDelay = StartCoroutine(QueueItemUse(item));
    // }

    // private IEnumerator QueueItemUse(Item item)
    // {
    //     yield return new WaitForSeconds(item.useTime);
    //     if (itemAction.IsInProgress())
    //     {
    //         item.AttemptUse(entity);
    //         co_itemDelay = null;
    //     }
    // }

}

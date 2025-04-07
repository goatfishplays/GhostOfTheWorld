using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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


    // private Coroutine co_itemDelay = null;
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
        itemAction = playerInput.actions.FindAction("Item");
        inventoryAction = playerInput.actions.FindAction("Inventory");
        interactAction = playerInput.actions.FindAction("Interact");
        shiftAction = playerInput.actions.FindAction("Shift");
        // lookAction.performed += context => { playerCameraControl.AddRotation(context.ReadValue<Vector2>()); };
        lookAction.performed += Look;
        dashAction.started += Dash;
        sprintAction.started += StartSprint;
        sprintAction.canceled += EndSprint;
        itemAction.performed += ProgressItemUse;
        itemAction.canceled += ItemKeyRelease;
        inventoryAction.started += ToggleInventory;
        interactAction.started += StartInteract;
        interactAction.canceled += EndInteract;

    }



    private void OnEnable()
    {
        movementAction.Enable();
        lookAction.Enable();
        dashAction.Enable();
        sprintAction.Enable();
        itemAction.Enable();
        inventoryAction.Enable();
        interactAction.Enable();
        shiftAction.Enable();
    }

    private void OnDisable()
    {
        movementAction.Disable();
        lookAction.Disable();
        dashAction.Disable();
        sprintAction.Disable();
        itemAction.Disable();
        inventoryAction.Disable();
        interactAction.Disable();
        shiftAction.Disable();
    }

    void OnDestroy()
    {
        lookAction.performed -= Look;
        dashAction.started -= Dash;
        sprintAction.started -= StartSprint;
        sprintAction.canceled -= EndSprint;
        itemAction.performed -= ProgressItemUse;
        itemAction.canceled -= ItemKeyRelease;
        inventoryAction.started -= ToggleInventory;
        interactAction.started -= StartInteract;
        interactAction.canceled -= EndInteract;
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

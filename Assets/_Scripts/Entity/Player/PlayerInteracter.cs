using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField] private Entity player;
    public Transform castPoint;
    public float maxCastDistance = 4f;
    private int layerMask;
    private EctoplasmDrop currentEctoplasmDrop = null;

    //private int interactableMask;
    //private Interactable target;
    //private float curInteractTime = 0;
    //[SerializeField] private float areaEctoplasmTime = 3f;
    //[SerializeField] private float areaRadius = 4f;

    //[DoNotSerialize] public bool interactionHeld = false;
    //[DoNotSerialize] public bool areaToggle = false;
    [SerializeField] private InteracterIndicater interacterIndicater;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Entities", "Interactable", "World");
        // interactableMask = LayerMask.GetMask("Interactable");
    }

    public void Start()
    {
        //if (interacterIndicater != null)
        //{
        //    Interactable interactable = AttemptCastToEctoplasm();
        //    SetInteractionBoi(interactable);
        //}
    }

    
    public void Update()
    {
        // Try to find a interactable in front of us
        currentEctoplasmDrop = AttemptCastToEctoplasm();

        /*
         GameObject hitBoi = AttemptCast(); 
         if (hitBoi != null)
         {
             // Debug.Log(hitBoi.name);
             Interactable hitInteractable = hitBoi.GetComponent<Interactable>();
         } 
         Interactable interactable;

        if (!areaToggle)
        {
            Interactable interactable = AttemptCast();
            if (interactable != target)
            {
                SetInteractionBoi(interactable);
            }
        }
        // Debug.Log("target: " + (target != null ? target.name : "null"));

        // If the target is something or area pickup
        if (target != null || areaToggle)
        {
            // Normal pickup
            if (interactionHeld)
            {

                curInteractTime += Time.deltaTime;
                if (areaToggle)
                {
                    interacterIndicater.fill.fillAmount = curInteractTime / areaEctoplasmTime;
                    if (curInteractTime > areaEctoplasmTime)
                    {
                        AreaEctoplasmInteract();

                        StartCoroutine(CastAndSetInteraction());
                    }
                }
                else
                {
                    interacterIndicater.fill.fillAmount = curInteractTime / target.interactableSO.interactionTime;
                    if (curInteractTime > target.interactableSO.interactionTime)
                    {
                        target.Interact(entity);

                        StartCoroutine(CastAndSetInteraction());
                    }
                }
            }
            // Try instant interact
            else if (curInteractTime > 0f)
            {
                if (areaToggle)
                {
                    AreaEctoplasmInstantInteract();

                }
                else if (target is EctoplasmDrop)
                {
                    (target as EctoplasmDrop).InstantInteract(entity);
                }
                StartCoroutine(CastAndSetInteraction());
            }
        }
        */
    }

    // Attempts to get and return the ectoplasm drop that the player is looking at
    public EctoplasmDrop AttemptCastToEctoplasm()
    {
        if (Physics.Raycast(castPoint.position, castPoint.forward, out RaycastHit hit, maxCastDistance, layerMask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent<EctoplasmDrop>(out var ectoplasmDrop))
            {
                Debug.Log("ectoplasm drop: " + ectoplasmDrop);
                interacterIndicater.fill.fillAmount = 1;
                return ectoplasmDrop;
            }
        }
        interacterIndicater.fill.fillAmount = 0;
        return null;
    }

    public void TryInstantUse()
    {
        if (currentEctoplasmDrop != null)
        {
            Debug.Log("Tried to get the thingy");
            currentEctoplasmDrop.InstantInteract(player);
        }
    }
    
    public void startInteract()
    {
        TryInstantUse();
    }

    public void endInteract()
    {
         // Does not do anything. Basic function for if needed later.
    }



    //private void AreaEctoplasmInteract()
    //{
    //    foreach (Collider col in Physics.OverlapSphere(castPoint.position, areaRadius, interactableMask, QueryTriggerInteraction.Collide))
    //    {
    //        if (col.TryGetComponent<EctoplasmDrop>(out EctoplasmDrop ectoplasmDrop))
    //        {
    //            ectoplasmDrop.Interact(player);
    //        }
    //    }
    //    areaToggle = false;
    //}
    //private void AreaEctoplasmInstantInteract()
    //{
    //    foreach (Collider col in Physics.OverlapSphere(castPoint.position, areaRadius, interactableMask, QueryTriggerInteraction.Collide))
    //    {
    //        if (col.TryGetComponent<EctoplasmDrop>(out EctoplasmDrop ectoplasmDrop))
    //        {
    //            ectoplasmDrop.InstantInteract(player);
    //        }
    //    }
    //    areaToggle = false;
    //}

    // obj not deleted till end of frame so need coroutine 
    //public IEnumerator CastAndSetInteraction()
    //{
    //    yield return new WaitForEndOfFrame();
    //    SetInteractionBoi(AttemptCastToEctoplasm());
    //}

    // Starts an interaction with an interactable
    //public void SetInteractionBoi(EctoplasmDrop interactable)
    //{
    //    // Debug.Log(interactable != null ? interactable.name : "null");
    //    //interacterIndicater.SetInteractable(interactable != null);
    //    //interacterIndicater.fill.fillAmount = 0;

    //    //target = interactable;

    //    //ResetInteractTime();
    //}


    //public void ResetInteractTime()
    //{
    //    curInteractTime = 0;
    //}
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField] private Entity entity;
    public Transform castPoint;
    public float maxCastDistance = 4f;
    private int layerMask;
    private int interactableMask;
    private Interactable target;
    private float curInteractTime = 0;
    [SerializeField] private float areaEctoplasmTime = 3f;
    [SerializeField] private float areaRadius = 4f;

    public bool interactionHeld = false;
    public bool areaToggle = false;
    [SerializeField] private InteracterIndicater interacterIndicater;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Entities", "Interactable", "World");
        interactableMask = LayerMask.GetMask("Interactable");
    }

    public void Start()
    {
        if (interacterIndicater != null)
        {
            Interactable interactable = AttemptCast();
            SetInteractionBoi(interactable);

        }
    }
    // public GameObject AttemptCast()
    public Interactable AttemptCast()
    {
        if (Physics.Raycast(castPoint.position, castPoint.forward, out RaycastHit hit, maxCastDistance, layerMask, QueryTriggerInteraction.Collide))
        {
            // Debug.Log($"{hit.collider.gameObject.name}");
            // return hit.collider.gameObject;
            return hit.collider.GetComponent<Interactable>();
        }
        return null;
    }


    public void Update()
    {
        // GameObject hitBoi = AttemptCast(); 
        // if (hitBoi != null)
        // {
        //     // Debug.Log(hitBoi.name);
        //     Interactable hitInteractable = hitBoi.GetComponent<Interactable>();
        // } 
        // Interactable interactable;
        if (!areaToggle)
        {
            Interactable interactable = AttemptCast();
            if (interactable != target)
            {
                SetInteractionBoi(interactable);
            }
        }
        // Debug.Log("target: " + (target != null ? target.name : "null"));

        if (target != null || areaToggle)
        {
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

    }

    private void AreaEctoplasmInteract()
    {
        foreach (Collider col in Physics.OverlapSphere(castPoint.position, areaRadius, interactableMask, QueryTriggerInteraction.Collide))
        {
            EctoplasmDrop ed = col.GetComponent<EctoplasmDrop>();
            if (ed != null)
            {
                ed.Interact(entity);
            }
        }
        areaToggle = false;
    }
    private void AreaEctoplasmInstantInteract()
    {
        foreach (Collider col in Physics.OverlapSphere(castPoint.position, areaRadius, interactableMask, QueryTriggerInteraction.Collide))
        {
            EctoplasmDrop ed = col.GetComponent<EctoplasmDrop>();
            // Debug.Log(col.gameObject.name);
            if (ed != null)
            {
                ed.InstantInteract(entity);
            }
        }
        areaToggle = false;
    }

    // obj not deleted till end of frame so need coroutine 
    public IEnumerator CastAndSetInteraction()
    {
        yield return new WaitForEndOfFrame();
        SetInteractionBoi(AttemptCast());
    }

    public void SetInteractionBoi(Interactable interactable)
    {
        // Debug.Log(interactable != null ? interactable.name : "null");
        interacterIndicater.SetInteractable(interactable != null);
        interacterIndicater.fill.fillAmount = 0;
        target = interactable;
        ResetInteractTime();
    }


    public void ResetInteractTime()
    {
        curInteractTime = 0;
    }
}

using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Defines how the interactable interacts with the player and non-player.")]
    public InteractableSO interactableSO;
    [Tooltip("The parent of the object this script is attached to. Used for destroying the interactable.")]
    public GameObject rootObject;

    public virtual void Interact(Entity interacter)
    {
        if (!interactableSO.nonPlayersInteractable && interacter.id != Entity.playerID)
        {
            return;
        }
        Debug.LogError($"Attempted to interact with {gameObject.name} which has no interaction implementation");

        if (interactableSO.destroyOnInteract)
        {
            Destroy(rootObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.name); 
        if (!interactableSO.interactOnContact)
        {
            return;
        }

        Entity entity = other.GetComponent<Entity>();
        if (entity == null)
        {
            return;
        }

        Interact(entity);
    }
}

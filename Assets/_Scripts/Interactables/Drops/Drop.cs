using System.Collections;
using UnityEngine;

public class Drop : Interactable
{
    [Tooltip("Defines the stats of the item.")]
    [SerializeField] protected ItemSO item;
    [Tooltip("The number of items in this drop.")]
    [SerializeField] protected int count = 1;
    protected Coroutine co_pickupLock = null;

    public void Initailize(ItemSO item, int count = 1, float pickupLockTime = 0f)
    {
        this.item = item;
        this.count = count;
        co_pickupLock = StartCoroutine(WaitForPickup(pickupLockTime));
        DropManager.instance.CacheDrop(this);
    }

    private IEnumerator WaitForPickup(float pickupLockTime)
    {
        yield return new WaitForSeconds(pickupLockTime);
        co_pickupLock = null;
    }



    public override void Interact(Entity interacter)
    {
        if (!interactableSO.nonPlayersInteractable && interacter.id != Entity.playerID || co_pickupLock != null)
        {
            return;
        }

        interacter.GetComponent<Inventory>().AddItem(item, count);
        Debug.Log($"Entity {interacter.name}, picked up {count} of {item.name}");

        if (interactableSO.destroyOnInteract)
        {
            Destroy(rootObject);
        }
    }

    void OnDestroy()
    {
        DropManager.instance.UnCacheDrop(this);
    }
}

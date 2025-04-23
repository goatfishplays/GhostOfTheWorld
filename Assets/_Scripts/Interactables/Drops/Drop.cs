using System.Collections;
using UnityEngine;

public class Drop : Interactable
{
    [SerializeField] protected ItemSO item;
    [SerializeField] protected int count = 1;
    protected Coroutine co_pickupLock = null;

    public void Initailize(ItemSO item, int count = 1, float pickupLockTime = 0f)
    {
        this.item = item;
        this.count = count;
        co_pickupLock = StartCoroutine(WaitForPickup(pickupLockTime));
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
}

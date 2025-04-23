using UnityEngine;

public class EctoplasmDrop : Drop
{
    private static float percentInstantHeal = 1f;
    public void InstantInteract(Entity interacter)
    {
        if (!interactableSO.nonPlayersInteractable && interacter.id != Entity.playerID || co_pickupLock != null)
        {
            return;
        }
        float healAmt = (item as PillItem).healAmount * percentInstantHeal;
        interacter.entityHealth.ChangeHealth(healAmt);
        Debug.Log($"Entity {interacter.name}, instant picked up {count} of {item.name} to heal {healAmt}");
        if (interactableSO.destroyOnInteract)
        {
            Destroy(rootObject);
        }
    }
}

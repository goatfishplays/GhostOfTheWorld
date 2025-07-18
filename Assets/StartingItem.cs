using System.Collections;
using UnityEngine;

public class StartingItem : MonoBehaviour
{
    public Inventory playerInventory;
    public ItemSO startingItem;

    private void Start()
    {
        
        StartCoroutine(delay(0.1f));
    }

    IEnumerator delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        playerInventory.AddItem(startingItem);
    }
}

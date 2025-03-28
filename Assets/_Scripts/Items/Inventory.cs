using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<Item, int> itemCounts = new Dictionary<Item, int>();
    public Entity entity;

    public bool HasEnough(Item item, int count)
    {
        return itemCounts.TryGetValue(item, out int val) && val >= count;
    }

    public int GetItemCount(Item item)
    {
        return itemCounts.GetValueOrDefault(item, 0);
    }

    public void AddItem(Item item, int count = 1)
    {
        if (!itemCounts.TryAdd(item, count))
        {
            itemCounts[item] += count;
        }
    }

    public void RemoveItem(Item item, int count = 1)
    {
        itemCounts[item] -= count;
        if (itemCounts[item] <= 0)
        {
            itemCounts.Remove(item);
        }
    }


    public bool AttemptUseItem(Item item)
    {
        if (item.AttemptUse(entity))
        {
            Debug.Log($"{gameObject.name} used Item: '{item.itemName}'");
            if (item.consumeOnUse)
            {
                RemoveItem(item);
            }
            return true;
        }
        return false;
    }
}

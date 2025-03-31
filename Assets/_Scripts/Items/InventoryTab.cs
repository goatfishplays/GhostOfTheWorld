using System.Collections.Generic;
using UnityEngine;

public class InventoryTab // TODO in retrospect this system is stupid, coulda just made it such that item links directly to count in inventory then has a secondary dict that links tabs to Item
{
    /// <summary> 
    /// Had to provide access to foreach over this, DO NOT MUTATE  
    /// </summary>
    public Dictionary<Item, int> itemCounts = new Dictionary<Item, int>();
    private string _tabName = "";

    public string tabName => _tabName;

    public InventoryTab(string tabName)
    {
        this._tabName = tabName;
        itemCounts = new Dictionary<Item, int>();
    }

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


    public bool AttemptUseItem(Item item, Entity entity)
    {
        if (GetItemCount(item) > 0 && item.AttemptUse(entity))
        {
            // Debug.Log($"{gameObject.name} used Item: '{item.itemName}'"); 
            if (item.consumeOnUse)
            {
                RemoveItem(item);
            }
            return true;
        }
        return false;
    }
}

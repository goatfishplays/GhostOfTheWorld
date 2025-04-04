using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, HashSet<ItemSO>> itemTabs = new Dictionary<string, HashSet<ItemSO>>();
    public Dictionary<ItemSO, int> itemCounts = new Dictionary<ItemSO, int>();
    private List<string> visibleTabs = new List<string> { "Items", "Key Items", "Beings" };
    public ItemSO primaryItem;
    [SerializeField] private ItemUIInventoryController controllerUI;
    [SerializeField] private ItemUICountElement primaryItemUI;
    public Entity entity;
    [SerializeField] float dropOffset = 1f;

    public void Start()
    {
        // initailize visible tabs 
        // itemTabs.Add("Items", new InventoryTab());
        // itemTabs.Add("Key Items", new InventoryTab());
        foreach (string tabName in visibleTabs)
        {
            // itemTabs.Add(tabName, new InventoryTab(tabName)); 
            itemTabs.Add(tabName, new HashSet<ItemSO>());
        }
    }

    public void Equip(ItemSO item)
    {
        primaryItem = item;
        primaryItemUI.SetItem(item, GetItemCount(item));
    }

    public void Drop(ItemSO item)
    {
        if (GetItemCount(item) <= 0)
        {
            Debug.LogWarning($"Attempted to drop item: '{item.name}' with 0 count");
            return;
        }
        RemoveItem(item);
        DropManager.instance.CreateDrop(entity.transform.position + dropOffset * entity.transform.forward, Quaternion.identity, item, 1, 1f);

        if (primaryItem == item)
        {
            primaryItemUI.SetCount(GetItemCount(item));
        }
    }

    public bool CanUsePrimary()
    {
        return primaryItem != null && GetItemCount(primaryItem) > 0;
    }

    public bool AttemptUsePrimary()
    {
        if (!AttemptUseItem(primaryItem))
        {
            return false;
        }
        int amtLeft = GetItemCount(primaryItem);
        // if (amtLeft > 0)
        // {
        primaryItemUI.SetCount(amtLeft);
        // } // TODO idrk if it should auto clear or how to manage that, someone remind me to make it so that when you have no item and pick up item it auto selects that one 
        // else
        // {
        //     primaryItemUI.ClearItem(); 
        // }
        return true;
    }

    public HashSet<ItemSO> GetTabItems(string tabName)
    {
        return itemTabs[tabName];
    }

    public HashSet<ItemSO> GetTabItems(int tabIndex)
    {
        // Debug.Log(tabIndex % visibleTabs.Count); // screw you C#
        int div = visibleTabs.Count;
        int ind = tabIndex % div;
        if (ind < 0)
        {
            ind += div;
        }
        return itemTabs[visibleTabs[ind]];
    }

    public string GetTabName(int tabIndex)
    {
        int div = visibleTabs.Count;
        int ind = tabIndex % div;
        if (ind < 0)
        {
            ind += div;
        }
        return visibleTabs[ind];
    }

    public bool HasEnough(ItemSO item, int count)
    {
        return itemCounts.TryGetValue(item, out int val) && val >= count;
        // return itemTabs.TryGetValue(item.tab, out InventoryTab tab) && tab.HasEnough(item, count);
    }

    public int GetItemCount(ItemSO item)
    {
        if (item == null)
        {
            return 0;
        }
        return itemCounts.GetValueOrDefault(item, 0);
        // return itemTabs.TryGetValue(item.tab, out InventoryTab tab) ? tab.GetItemCount(item) : 0;
    }

    public void AddItem(ItemSO item, int count = 1)
    {
        // if (!itemTabs.TryGetValue(item.tab, out InventoryTab tab))
        // {
        //     itemTabs.Add(item.tab, new InventoryTab(item.tab));
        //     tab = itemTabs[item.tab];
        //     // return;
        // }
        // tab.AddItem(item, count);

        if (!itemCounts.TryAdd(item, count))
        {
            itemCounts[item] += count;
        }
        else
        {
            itemTabs[item.tab].Add(item);
        }

        if (controllerUI && controllerUI.gameObject.activeInHierarchy)
        {
            controllerUI.UpdateScreen(GetTabItems(controllerUI.curTab), GetTabName(controllerUI.curTab), false);
        }

        if (item == primaryItem)
        {
            Equip(primaryItem);
        }
    }

    public void RemoveItem(ItemSO item, int count = 1)
    {
        if (GetItemCount(item) <= 0)
        {
            Debug.LogWarning($"Attempted to remove item: '{item.name}' with 0 count");
            return;
        }
        itemCounts[item] -= count;
        if (itemCounts[item] <= 0)
        {
            itemCounts.Remove(item);
            itemTabs[item.tab].Remove(item);
        }
        if (controllerUI && controllerUI.gameObject.activeInHierarchy)
        {
            controllerUI.UpdateScreen(GetTabItems(controllerUI.curTab), GetTabName(controllerUI.curTab), !itemCounts.ContainsKey(item));
        }
        // if (!itemTabs.TryGetValue(item.tab, out InventoryTab tab))
        // {
        //     return;
        // }
        // tab.RemoveItem(item, count);
    }


    public bool AttemptUseItem(ItemSO item)
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
        // return itemTabs.TryGetValue(item.tab, out InventoryTab tab) && tab.AttemptUseItem(item, entity);
    }



}

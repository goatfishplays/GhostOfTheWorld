using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, InventoryTab> itemTabs = new Dictionary<string, InventoryTab>();
    private List<string> visibleTabs = new List<string> { "Items", "Key Items", "Beings" };
    public Item primaryItem;
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
            itemTabs.Add(tabName, new InventoryTab(tabName));
        }
    }

    public void Equip(Item item)
    {
        primaryItem = item;
        primaryItemUI.SetItem(item, GetItemCount(item));
    }

    public void Drop(Item item)
    {
        RemoveItem(item);
        Instantiate(item.dropPrefab, entity.transform.position + dropOffset * entity.transform.forward, Quaternion.identity, HierarchyGroups.instance.drops);
        if (primaryItem == item)
        {
            primaryItemUI.SetCount(GetItemCount(item));
        }
    }

    public bool AttemptUsePrimary()
    {
        if (!AttemptUseItem(primaryItem))
        {
            return false;
        }
        int amtLeft = GetItemCount(primaryItem);
        if (amtLeft > 0)
        {
            primaryItemUI.SetCount(amtLeft);
        } // TODO idrk if it should auto clear or how to manage that, someone remind me to make it so that when you have no item and pick up item it auto selects that one 
        // else
        // {
        //     primaryItemUI.ClearItem(); 
        // }
        return true;
    }

    public InventoryTab GetTab(string tabName)
    {
        return itemTabs[tabName];
    }

    public InventoryTab GetTab(int tabIndex)
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

    public bool HasEnough(Item item, int count)
    {
        return itemTabs.TryGetValue(item.tab, out InventoryTab tab) && tab.HasEnough(item, count);
    }

    public int GetItemCount(Item item)
    {
        return itemTabs.TryGetValue(item.tab, out InventoryTab tab) ? tab.GetItemCount(item) : 0;
    }
    public void AddItem(Item item, int count = 1)
    {
        if (!itemTabs.TryGetValue(item.tab, out InventoryTab tab))
        {
            itemTabs.Add(item.tab, new InventoryTab(item.tab));
            tab = itemTabs[item.tab];
            // return;
        }
        tab.AddItem(item, count);
    }

    public void RemoveItem(Item item, int count = 1)
    {
        if (!itemTabs.TryGetValue(item.tab, out InventoryTab tab))
        {
            return;
        }
        tab.RemoveItem(item, count);
    }


    public bool AttemptUseItem(Item item)
    {
        return itemTabs.TryGetValue(item.tab, out InventoryTab tab) && tab.AttemptUseItem(item, entity);
    }



}

using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //! don't think this is currently used within the game, would mostly be used if we ever got a console to dynamically search for items for some reason
    public static ItemManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple ItemManagers detected, deleting extra");
            Destroy(this);
        }
        InitializeItems();
    }

    private void InitializeItems()
    {
        // TestItemType test1 = new TestItemType("Test1", "This is the description of test1", null, 5);
        // AddItem(test1);
        // TestItemType test2 = new TestItemType("Test2", "This is the description of test2", null, -5);
        // AddItem(test2);

    }


    private Dictionary<string, int> nameToInd = new Dictionary<string, int>();
    private List<ItemSO> itemCatalogue = new List<ItemSO>();

    public void AddItem(ItemSO item)
    {
        if (nameToInd.ContainsKey(item.name))
        {
            Debug.LogError($"Attempted to add '{item.name}' to Item Manager where '{item.name}' already exists");
            return;
        }
        int newInd = nameToInd.Count;
        nameToInd.Add(item.name, newInd);
        itemCatalogue.Add(item);
        Debug.Log($"Initial Task: Added item: '{item.name}' with item index: {newInd}");
    }

    public ItemSO GetItem(string name)
    {
        int id = -1;
        if (nameToInd.TryGetValue(name, out id))
        {
            return GetItem(id);
        }
        Debug.LogError($"Attempted to get item with name, '{name}', where it does not exist");
        return null;
    }

    public ItemSO GetItem(int id)
    {
        if (id >= 0 && id < itemCatalogue.Count)
        {
            return itemCatalogue[id];
        }
        Debug.LogError($"Attempted to get item with id, '{id}', where it does not exist");
        return null;
    }

}

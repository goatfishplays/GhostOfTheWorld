using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
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
    private List<Item> itemCatalogue = new List<Item>();

    public void AddItem(Item item)
    {
        if (nameToInd.ContainsKey(item.itemName))
        {
            Debug.LogError($"Attempted to add '{item.itemName}' to Item Manager where '{item.itemName}' already exists");
            return;
        }
        int newInd = nameToInd.Count;
        nameToInd.Add(item.itemName, newInd);
        itemCatalogue.Add(item);
        Debug.Log($"Initial Task: Added item: '{item.itemName}' with item index: {newInd}");
    }

    public Item GetItem(string name)
    {
        int id = -1;
        if (nameToInd.TryGetValue(name, out id))
        {
            return GetItem(id);
        }
        Debug.LogError($"Attempted to get item with name, '{name}', where it does not exist");
        return null;
    }

    public Item GetItem(int id)
    {
        if (id >= 0 && id < itemCatalogue.Count)
        {
            return itemCatalogue[id];
        }
        Debug.LogError($"Attempted to get item with id, '{id}', where it does not exist");
        return null;
    }

}

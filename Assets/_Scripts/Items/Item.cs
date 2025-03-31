using UnityEngine;

[System.Serializable]
public class Item
{

    public string itemName = "unnamed";
    /// <summary>
    /// Will control where it gets sorted
    /// </summary>
    public string tab = "Items";
    public string description = "no description";
    public bool consumeOnUse = true;
    public bool dropable = false;
    public bool equipable = false;
    public GameObject dropPrefab;
    public Sprite sprite;

    // Only need to use this if we decide we want to load resources at runtime rather than at editortime(is it called editor time? idrk, maybe it is compile time for this too), would allow for resource packs lmaoooo we so don't need it do we...
    public Item(string itemName, string description, bool consumeOnUse, Sprite sprite)
    {
        this.itemName = itemName;
        this.description = description;
        this.consumeOnUse = consumeOnUse;
        this.sprite = sprite;
    }

    public virtual bool AttemptUse(Entity user)
    {
        Debug.LogWarning($"{user.gameObject.name} attempted to use '{itemName}' without item implementation");
        return false;
    }
}

using UnityEngine;

// [System.Serializable]
[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    // will just use name of file thing cause easier   
    // public string name = "unnamed"; 

    /// <summary>
    /// Will control where it gets sorted
    /// </summary>
    public string tab = "Items";
    public string description = "no description";
    public bool consumeOnUse = true;
    public bool dropable = false;
    public bool equipable = false;
    public float useTime = 0f;
    public float useMovementSpeedMult = 0.25f;
    public GameObject dropPrefab;
    public Sprite sprite;

    // Only need to use this if we decide we want to load resources at runtime rather than at editortime(is it called editor time? idrk, maybe it is compile time for this too), would allow for resource packs lmaoooo we so don't need it do we...
    public ItemSO(string itemName, string description, bool consumeOnUse, Sprite sprite)
    {
        this.name = itemName;
        this.description = description;
        this.consumeOnUse = consumeOnUse;
        this.sprite = sprite;
    }

    public virtual bool AttemptUse(Entity user)
    {
        Debug.LogWarning($"{user.gameObject.name} attempted to use '{name}' without item implementation");
        return false;
    }
}

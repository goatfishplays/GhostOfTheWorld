using UnityEngine;

/// <summary>
/// Adds items to the ItemManager
/// </summary>
public class PillItemAdder : MonoBehaviour
{
    public PillItemSO[] items;
    void Start()
    {
        foreach (var item in items)
        {
            ItemManager.instance.AddItem(item);
        }
        // Destroy(gameObject);  
    }

}

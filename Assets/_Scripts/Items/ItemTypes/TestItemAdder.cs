using UnityEngine;

/// <summary>
/// Adds items to the ItemManager
/// </summary>
public class TestItemAdder : MonoBehaviour
{
    public TestItemType[] items;
    void Start()
    {
        foreach (var item in items)
        {
            ItemManager.instance.AddItem(item);
        }
        Destroy(gameObject);
    }

}

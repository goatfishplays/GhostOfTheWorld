using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Tooltip("The game object that is the parent of all of the drops. Usually --- Drops ---")]
    [SerializeField] private Transform holder;
    private static DropManager _instance;
    public static DropManager instance => _instance;
    private HashSet<Drop> currentDrops = new HashSet<Drop>();
    [SerializeField] private GameObject dropBase;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple DropManagers Detected Deleting Second");
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Creates a drop from the information contained in the itemSO
    /// </summary>
    /// <param name="pos">Position to spawn drop</param>
    /// <param name="rot">Rotation of drop</param>
    /// <param name="item">ItemSO containing the prefab to instantiate</param>
    /// <param name="count">Amount of the item upon pickup</param>
    /// <param name="lockTime">Amount of time before the item can be picked up(mainly useful if interactOnContact is set true in InteractableSO)</param>
    public void CreateDrop(Vector3 pos, Quaternion rot, ItemSO item, int count = 1, float lockTime = 0.1f)
    {
        GameObject spawned = Instantiate(item.dropPrefab, pos, rot, holder);
        Drop spawnedDrop = spawned.GetComponentInChildren<Drop>();
        spawnedDrop.Initailize(item, count, lockTime);
    }

    /// <summary> //! if later get other types of drops with instant interacts than can do that instead of PillItemSO 
    /// Creates a ectoplasm drop from the information contained in the itemSO
    /// Allows instantInteract function to work
    /// </summary>
    /// <param name="pos">Position to spawn drop</param>
    /// <param name="rot">Rotation of drop</param> 
    /// <param name="item">ItemSO containing the prefab to instantiate</param>
    /// <param name="count">Amount of the item upon pickup</param>
    /// <param name="lockTime">Amount of time before the item can be picked up(mainly useful if interactOnContact is set true in InteractableSO)</param>
    public void CreateEctoplasmDrop(Vector3 pos, Quaternion rot, PillItemSO item, int count = 1, float lockTime = 0.1f)
    {
        GameObject spawned = Instantiate(item.dropPrefab, pos, rot, holder);
        Drop spawnedDrop = spawned.GetComponentInChildren<EctoplasmDrop>();
        spawnedDrop.Initailize(item, count, lockTime);
    }

    public void CacheDrop(Drop drop)
    {
        currentDrops.Add(drop);
    }

    public void UnCacheDrop(Drop drop)
    {
        currentDrops.Remove(drop);
    }


    public void PickupAllDrops(Entity entity)
    {
        foreach (Drop drop in currentDrops)
        {
            drop.Interact(entity);
        }
    }

    public void DestoryAllDrops()
    {
        foreach (Drop drop in currentDrops)
        {
            Destroy(drop.gameObject);
        }
        currentDrops.Clear();
    }
}

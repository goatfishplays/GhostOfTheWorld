using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Tooltip("The game object that is the parent of all of the drops. Usually --- Drops ---")]
    [SerializeField] private Transform holder;
    private static DropManager _instance;
    public static DropManager instance => _instance;
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
        spawned.GetComponentInChildren<Drop>().Initailize(item, count, lockTime);
    }

    /// <summary>
    /// Creates a ectoplasm drop from the information contained in the itemSO
    /// Allows instantInteract function to work
    /// </summary>
    /// <param name="pos">Position to spawn drop</param>
    /// <param name="rot">Rotation of drop</param> 
    /// <param name="item">ItemSO containing the prefab to instantiate</param>
    /// <param name="count">Amount of the item upon pickup</param>
    /// <param name="lockTime">Amount of time before the item can be picked up(mainly useful if interactOnContact is set true in InteractableSO)</param>
    public void CreateEctoplasmDrop(Vector3 pos, Quaternion rot, ItemSO item, int count = 1, float lockTime = 0.1f)
    {
        GameObject spawned = Instantiate(item.dropPrefab, pos, rot, holder);
        spawned.GetComponentInChildren<EctoplasmDrop>().Initailize(item, count, lockTime);
    }
}

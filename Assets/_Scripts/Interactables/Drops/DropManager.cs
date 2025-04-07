using UnityEngine;

public class DropManager : MonoBehaviour
{
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

    public void CreateDrop(Vector3 pos, Quaternion rot, ItemSO item, int count = 1, float lockTime = 0.1f)
    {
        GameObject spawned = Instantiate(item.dropPrefab, pos, rot, holder);
        spawned.GetComponentInChildren<Drop>().Initailize(item, count, lockTime);
    }
}

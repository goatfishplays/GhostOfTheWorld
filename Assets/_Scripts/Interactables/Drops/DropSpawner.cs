using Unity.VisualScripting;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    abstract class DropInfoBase
    {
        [Tooltip("Minimum amount in the stack of items.", order = 10)]
        [Min(0)]
        public int minCount;
        [Range(0f, 1f)]
        [Tooltip("Percentage chance within 0 to 1 inclusive of another drop generating above the minCount.", order = 10)]
        public float excessDropChance;
        [Tooltip("Max amount of items in the stack. Each maxCount greater than minCount is another roll using excessDropChance.", order = 10)]
        [Min(0)]
        public int maxCount;
    }

    [System.Serializable]
    class ItemDropInfo : DropInfoBase
    {
        [Tooltip("Scriptable object which defines the physical object and items stats", order = 1)]
        public ItemSO itemSO;
    }
    [System.Serializable]
    class EctoplasmDropInfo : DropInfoBase
    {
        [Tooltip("Scriptable object which defines the physical object and ectoplasm stats", order = 1)]
        public PillItemSO itemSO;
    }

    [SerializeField] private ItemDropInfo[] drops;
    [SerializeField] private EctoplasmDropInfo[] ectoplasmDrops;
    [SerializeField] private float dropRadius = .5f;
    [SerializeField] private Vector2 dropHeightRange = new Vector2(-0.2f, 0.5f);
    private const int maxDropLocationTries = 5;

    [SerializeField] private EntityHealth entityHealth;

    private void Start()
    {
        if (entityHealth == null)
        {
            Debug.LogWarning("entityHealth not set for DropSpawner. Trying to get component automatically.");
            entityHealth = gameObject.GetComponent<EntityHealth>();
            if (entityHealth == null)
            {
                return;
            }
        }

        entityHealth.OnDie += CreateDrops;
    }

    private void OnDestroy()
    {
        if (entityHealth != null)
        {
            entityHealth.OnDie -= CreateDrops;
        }
    }

    /// <summary>
    /// Generates drops 
    /// </summary>
    public void CreateDrops()
    {
        // Debug.Log("drop");
        foreach (var dropInfo in drops)
        {
            // Get Drop Position
            Vector3 dropPos;
            int dropTries = 0;
            do
            {
                if (dropTries < maxDropLocationTries)
                {
                    dropPos = GetDropPosition();
                    dropTries++;
                }
                else
                {
                    dropPos = transform.position;
                    break;
                }
            } while (Physics.OverlapSphere(dropPos, .5f, LayerMask.GetMask("World")).Length > 0);

            // Get Drop Count
            int dropCount = dropInfo.minCount;
            for (int i = dropInfo.minCount; i < dropInfo.maxCount; i++)
            {
                if (Random.value <= dropInfo.excessDropChance)
                {
                    dropCount++;
                }
            }

            DropManager.instance.CreateDrop(dropPos, Quaternion.identity, dropInfo.itemSO, dropCount, 0);
        }


        foreach (EctoplasmDropInfo dropInfo in ectoplasmDrops)
        {
            // Get Drop Position
            Vector3 dropPos;
            int dropTries = 0;
            do
            {
                if (dropTries < maxDropLocationTries)
                {
                    dropPos = GetDropPosition();
                    dropTries++;
                }
                else
                {
                    dropPos = transform.position;
                    break;
                }
            } while (Physics.OverlapSphere(dropPos, .5f, LayerMask.GetMask("World")).Length > 0);

            // Get Drop Count
            int dropCount = dropInfo.minCount;
            for (int i = dropInfo.minCount; i < dropInfo.maxCount; i++)
            {
                if (Random.value <= dropInfo.excessDropChance)
                {
                    dropCount++;
                }
            }

            DropManager.instance.CreateEctoplasmDrop(dropPos, Quaternion.identity, dropInfo.itemSO, dropCount, 0);
        }
    }

    private Vector3 GetDropPosition()
    {
        float dropDist = Random.Range(0, dropRadius);
        float dropAngle = Random.Range(0, 2 * Mathf.PI);
        return new Vector3(Mathf.Cos(dropAngle) * dropDist, Mathf.Sin(dropAngle) * dropDist, Random.Range(dropHeightRange.x, dropHeightRange.y)) + transform.position;
    }
}

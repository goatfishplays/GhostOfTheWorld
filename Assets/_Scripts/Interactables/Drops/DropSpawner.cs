using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [System.Serializable]
    struct DropInfo
    {
        public ItemSO itemSO;
        public int minCount;
        public float excessDropChance;
        public int maxCount;
    }
    [System.Serializable]
    struct EctoplasmDropInfo
    {
        public PillItemSO itemSO;
        public int minCount;
        public float excessDropChance;
        public int maxCount;
    }

    [SerializeField] private DropInfo[] drops;
    [SerializeField] private EctoplasmDropInfo[] ectoplasmDrops;
    [SerializeField] private float dropRadius = .5f;
    [SerializeField] private Vector2 dropHeightRange = new Vector2(-0.2f, 0.5f);
    private const int maxDropLocationTries = 5;

    [SerializeField] private EntityHealth entity;

    private void Start()
    {
        if (entity != null)
        {
            entity.OnDie += CreateDrops;
        }
    }

    private void OnDestroy()
    {
        entity.OnDie -= CreateDrops;
    }

    /// <summary>
    /// Generates drops 
    /// </summary>
    public void CreateDrops()
    {
        // Debug.Log("drop");
        foreach (DropInfo dropInfo in drops)
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

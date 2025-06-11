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
    public void CreateDrops()
    {
        foreach (DropInfo dropInfo in drops)
        {
            Vector3 dropPos;
            do
            {
                dropPos = GetDropPosition();
                if (Physics.OverlapSphere(transform.position, .5f, LayerMask.GetMask("World")).Length > 0)
                {
                    return;
                }
            } while (dropPos == null);
        }
    }

    private Vector3 GetDropPosition()
    {
        float dropDist = Random.Range(0, dropRadius);
        float dropAngle = Random.Range(0, 2 * Mathf.PI);
        return new Vector3(Mathf.Cos(dropAngle) * dropDist, Mathf.Sin(dropAngle) * dropDist, Random.Range(dropHeightRange.x, dropHeightRange.y));
    }
}

using UnityEngine;

public class HierarchyGroups : MonoBehaviour
{
    public static HierarchyGroups instance;
    [SerializeField] private Transform _world;
    public Transform world => _world;
    [SerializeField] private Transform _entities;
    public Transform entities => _entities;
    [SerializeField] private Transform _drops;
    public Transform drops => _drops;
    [SerializeField] private Transform _attacks;
    public Transform attacks => _attacks;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two Hierarchy Groupers Detected Deleting Second");
        }
    }
}

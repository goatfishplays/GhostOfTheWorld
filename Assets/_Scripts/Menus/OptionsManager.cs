using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two OptionsManagers detected deleting second");
            Destroy(this);
        }
    }

    [SerializeField] private float _xSensitivity = 1f;
    public float xSensitivity => _xSensitivity;
    public void SetXSensitivity(float xSensitivity)
    {
        _xSensitivity = xSensitivity;
    }

    [SerializeField] private float _ySensitivity = 1f;
    public float ySensitivity => _ySensitivity;
    public void SetYSensitivity(float ySensitivity)
    {
        _ySensitivity = ySensitivity;
    }


}

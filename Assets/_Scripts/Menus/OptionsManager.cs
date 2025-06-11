using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager instance;

    [SerializeField] private InputSliderSync xSensitivityHolder;
    [SerializeField] private float _xSensitivity;
    public float xSensitivity => _xSensitivity;
    public void SetXSensitivity(float xSensitivity)
    {
        xSensitivityHolder.SetValue(xSensitivity);
        _xSensitivity = xSensitivity;
    }

    public void FetchXSensitivity()
    {
        _xSensitivity = xSensitivityHolder.curVal;
    }
    [SerializeField] private InputSliderSync ySensitivityHolder;
    [SerializeField] private float _ySensitivity;
    public float ySensitivity => _ySensitivity;
    public void SetYSensitivity(float ySensitivity)
    {
        ySensitivityHolder.SetValue(ySensitivity);
    }

    public void FetchYSensitivity()
    {
        _ySensitivity = ySensitivityHolder.curVal;
        _ySensitivity = ySensitivity;
    }



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

        // xSensitivityHolder.onValueChanged.AddListener(FetchXSensitivity);
        // ySensitivityHolder.onValueChanged.AddListener(FetchYSensitivity); 
        xSensitivityHolder.onValueChanged.AddListener((float _) => FetchXSensitivity());
        ySensitivityHolder.onValueChanged.AddListener((float _) => FetchYSensitivity());
    }
}

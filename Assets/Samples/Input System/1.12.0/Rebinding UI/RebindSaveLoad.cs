using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;

    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
        // print(rebinds); 
    }

    public void OnDisable()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        // print(rebinds);
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}

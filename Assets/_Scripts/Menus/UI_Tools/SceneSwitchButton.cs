using UnityEngine;

public class SceneSwitchButton : MonoBehaviour
{
    [Tooltip("The name of the scene to load when the button is pressed. If empty will load the current scene.")]
    public string sceneName = "MainMenu";

    public void OnClick()
    {
        SceneSwitcher.SwitchScene(sceneName);
    }
}

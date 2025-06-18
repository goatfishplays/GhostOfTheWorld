using UnityEngine;

public class SceneSwitchButton : MonoBehaviour
{
    public string scene = "MainMenu";

    public void OnClick()
    {
        SceneSwitcher.SwitchScene(scene);
    }
}

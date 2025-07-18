using System.Collections;
using UnityEngine;

public class MainMenuHelper : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SetMenuStart());
    }

    private IEnumerator SetMenuStart()
    {
        yield return null;
        MenuManager.instance.SetState(MenuManager.MenuState.Pause);
        PlayerManager.instance.LockInputs(true, true, true);
    }

    // TODO Replace SampleScene with correct scene
    public void StartGame()
    {
        MenuManager.instance.SetState(MenuManager.MenuState.None);
        PlayerManager.instance.LockInputs(false, false, false);
        SceneSwitcher.SwitchScene("OfficialTest");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    // Loads the given sceneName. If sceneName is empty loads the active scene.
    public static void SwitchScene(string sceneName)
    {
        if (sceneName == "")
        {
            // Loads the current scene again.
            // This might be bad code practice idk
            sceneName = SceneManager.GetActiveScene().name;
            Debug.LogWarning("sceneName is empty, reloading current scene " + sceneName);
        }

        SceneManager.LoadScene(sceneName);
    }

}

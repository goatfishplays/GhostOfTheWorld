using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gideon_Temp : MonoBehaviour
{
    public Button reset;
    public GameObject UI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reset.onClick.AddListener(ResetScene);
        UI.SetActive(true); // Show the UI again. I don't want to see it in the editor.
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}

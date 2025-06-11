using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gideon_Temp : MonoBehaviour
{
    public Button reset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reset.onClick.AddListener(ResetScene);

    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}

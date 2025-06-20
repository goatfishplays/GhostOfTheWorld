using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gideon_Temp : MonoBehaviour
{
    public GameObject UI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UI.SetActive(true); // Show the UI again. I don't want to see it in the editor.
    }
}

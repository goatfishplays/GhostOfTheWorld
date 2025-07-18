using TMPro;
using UnityEngine;

public class BaseTextUI : MonoBehaviour
{
    public TextMeshProUGUI textBox = null;
    public string defaultText = "Wave";

    public void UpdateText(string newText)
    {
        textBox.text = newText;
    }

}

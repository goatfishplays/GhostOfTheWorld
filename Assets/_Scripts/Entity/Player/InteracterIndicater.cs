
using UnityEngine;
using UnityEngine.UI;

public class InteracterIndicater : MonoBehaviour
{
    public Image baseIcon;
    public Image fill;
    private Color interactIndicatorColor = new Color(1,1,1,0.1f);


    public void SetInteractable(bool state)
    {
        if (state)
        {
            baseIcon.color = interactIndicatorColor;
        }
        else
        {
            baseIcon.color = Color.clear;
        }
    }


}

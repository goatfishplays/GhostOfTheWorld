
using UnityEngine;
using UnityEngine.UI;

public class InteracterIndicater : MonoBehaviour
{
    public Image baseIcon;
    public Image fill;


    public void SetInteractable(bool state)
    {
        if (state)
        {
            baseIcon.color = Color.white;
        }
        else
        {
            baseIcon.color = Color.clear;
        }
    }


}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIController : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI countText;

    public void LoadItem(Item item, int amt = 1)
    {
        icon.sprite = item.sprite;
        if (nameText != null)
        {
            nameText.text = item.itemName;
        }
        if (descText != null)
        {
            nameText.text = item.itemName;
        }
        if (countText != null)
        {
            if (item.consumeOnUse)
            {
                countText.text = amt.ToString();
            }
            else
            {
                countText.text = "";
            }
        }
    }

    public void ClearItem()
    {
        icon.sprite = null;
        if (nameText != null)
        {
            nameText.text = "";
        }
        if (descText != null)
        {
            nameText.text = "";
        }
        if (countText != null)
        {
            countText.text = "";
        }
    }
}

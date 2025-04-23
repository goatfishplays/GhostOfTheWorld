using UnityEngine;
using UnityEngine.UI;

public class RonanTempThing : MonoBehaviour
{
    // public Entity tempEntity;
    // public Inventory inventory;
    // public ItemUIInventoryController itemUIInventoryController;
    // [SerializeField] private GridLayoutGroup itemIconsHolderGLG;
    public AudioClip[] sfx;
    public AudioClip[] music;
    public int sfxDelay = 1;
    public int musicDelay = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // // Debug.Log(inventory == null);

        // inventory.AddItem(ItemManager.instance.GetItem("Test1"), 3);
        // // inventory.AddItem(ItemManager.instance.GetItem("Test2")); 
        // // inventory.AddItem(ItemManager.instance.GetItem("Test3"));
        // // inventory.AddItem(ItemManager.instance.GetItem("Test4"));   
        // // inventory.AddItem(ItemManager.instance.GetItem("Test5"));
        // // inventory.AddItem(ItemManager.instance.GetItem("Test6"));
        // // inventory.AddItem(ItemManager.instance.GetItem("Test7"));
        // // inventory.AddItem(ItemManager.instance.GetItem("Test8"));
        // // inventory.AddItem(ItemManager.instance.GetItem("Test9"));
        // // inventory.AddItem(ItemManager.instance.GetItem("Test3"), 99);
        // for (int i = 0; i < 30; i++)
        // {
        //     ItemManager.instance.AddItem(new TestItemType(i.ToString()));
        //     inventory.AddItem(ItemManager.instance.GetItem(i.ToString()));
        // }

        // // itemUIInventoryController.UpdateScreen(inventory.GetTab("Items"));
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        if (sfxDelay <= 0)
        {
            AudioManager.instance.PlaySFXAtPoint(sfx[Random.Range(0, sfx.Length - 1)], new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Quaternion.identity);
            sfxDelay = Random.Range(0, 15);
        }
        else
        {
            sfxDelay -= 1;
        }
        if (musicDelay <= 0)
        {
            AudioManager.instance.FadeInMusic(music[Random.Range(0, music.Length - 1)]);
            musicDelay = Random.Range(0, 500);
        }
        else
        {
            musicDelay -= 1;
        }

        // Debug.Log(itemIconsHolderGLG.flexibleHeight);
        // Debug.Log(itemIconsHolderGLG.preferredHeight);
        // Debug.Log(itemIconsHolderGLG.minHeight);
    }
}

using UnityEngine;

public class RonanTempThing : MonoBehaviour
{
    public Entity tempEntity;
    public Inventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventory.AddItem(ItemManager.instance.GetItem("Test1"));
        inventory.AddItem(ItemManager.instance.GetItem("Test2"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (Input.GetKey(KeyCode.W))
        // {
        //     tempEntity.entityMovement.Move(Vector2.right);
        // }
        // else
        // {
        //     tempEntity.entityMovement.Move(Vector2.zero);
        // }
    }
}

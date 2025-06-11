using UnityEngine;


[CreateAssetMenu(fileName = "ItemSO", menuName = "Items/Pill")]
public class PillItemSO : ItemSO
{
    public float healAmount;
    public float toleranceAmount;

    public PillItemSO(string itemName, string description = "no desc", bool consumeOnUse = true, Sprite sprite = null, float healAmount = 5, float toleranceAmount = 1f) : base(itemName, description, consumeOnUse, sprite)
    {
        this.healAmount = healAmount;
        this.toleranceAmount = toleranceAmount;
    }

    public override bool AttemptUse(Entity user)
    {
        Debug.Log($"{user.gameObject.name} used {name} to change health by {healAmount}");
        // user.entityHealth.ChangeHealth(healAmount, 0f, true);
        user.GetComponent<ToleranceManager>().Heal(healAmount, toleranceAmount);
        return true;
    }
}

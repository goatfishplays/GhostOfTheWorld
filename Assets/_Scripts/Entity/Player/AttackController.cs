using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// This is designed for the player only.
// Any enemy or NPC would use their own AI for attack delay.
// TODO: Make this into a base class and then derive for new weapons.
public class AttackController : MonoBehaviour
{
    public EntityHealth playerHealth;
    public ProjectileSpawner projectileSpawner;

    // May need to move these stats to another place if we end up with multiple guns.
    // Likely an ScriptableObject with all the stats ends up wherever is needed.
    public float attackDelay = 0.2f;
    public float attackHealthCost = 1;

    private bool held = false;
    private Coroutine co_attacking = null;
    
    private void Start()
    {
        if (projectileSpawner == null)
        {
            Debug.LogWarning("AttackController has no projectileSpawner. Disabling AttackController");
            this.GetComponent<AttackController>().enabled = false;
        }
    }

    public void StartAttacking()
    {
        held = true;
        // Don't start attacking if attacking coroutine is active.
        if (co_attacking != null)
        {
            return;
        }
        co_attacking = StartCoroutine(Attacking());
    }

    public void EndAttacking()
    {
        held = false;
    }

    // Attempts to attack if player has enough health.
    private bool AttemptAttack()
    {
        if (playerHealth.GetHealth() - attackHealthCost <= 0)
        {
            Debug.Log("Player cannot fire gun that would cause their health to go below 0.");
            return false;
        }
        playerHealth.ChangeHealth(-attackHealthCost, 0, true);

        projectileSpawner.Shoot();
        return true;
    }

    private IEnumerator Attacking()
    {
        // Continues attacking if attack button is held.
        while (held)
        {
            AttemptAttack();

            // Handles cooldown between shots
            yield return new WaitForSeconds(attackDelay);
        }
        co_attacking = null;
    }
}

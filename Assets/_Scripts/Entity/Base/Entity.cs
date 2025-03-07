using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public int id = 0;

    public EntityHealth entityHealth;
    public EntityMovement entityMovement;

    [Header("Components")]
    public Rigidbody rb;
    public Collider hitBox;
    public Animator animator;

    // [Header("Status")]
    [Header("Die")]
    public GameObject dieDestroyObj;
    public float dieDestroyTime = 3f;
    public bool disableHitboxWhileDying = true;
    public GameObject[] enableOnDie;
    public GameObject[] disableOnDie;





    // Start is called before the first frame update 
    protected virtual void Start()
    {
        entityHealth.OnDie += Die;
    }

    protected virtual void OnDestroy()
    {
        entityHealth.OnDie -= Die;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }


    protected virtual void Die()
    {
        if (disableHitboxWhileDying)
        {
            hitBox.enabled = false;
        }
        if (entityMovement)
        {
            entityMovement.canMove = false;
        }
        foreach (GameObject go in enableOnDie)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in disableOnDie)
        {
            go.SetActive(false);
        }
        StartCoroutine(DieDestroyTimer());
    }

    private IEnumerator DieDestroyTimer()
    {
        yield return new WaitForSeconds(dieDestroyTime);
        // if (this == Player.instance.entity)
        // {
        //     SceneSwitcher.SwitchScene("GameOver");
        // }
        Destroy(dieDestroyObj);
    }

    public virtual void ApplyKnockback(Vector2 kb)
    {
        rb.AddForce(kb, ForceMode.Impulse);
    }

}

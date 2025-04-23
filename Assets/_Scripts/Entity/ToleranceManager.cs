using UnityEngine;
using UnityEngine.UI;

public class ToleranceManager : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private EntityHealth health;
    [SerializeField] private float _curTolerance;
    [SerializeField] private float maxTolerance;
    [SerializeField] private float tolerenceDecRate;


    // Update is called once per frame
    void Update()
    {
        if (_curTolerance > 0)
        {
            _curTolerance -= Time.deltaTime; // should be a negligable amount of extra lowerage bellow 0
            if (fill != null)
            {
                fill.fillAmount = _curTolerance / maxTolerance;
            }
        }
    }

    public void Heal(float rawHealDelta, float tolerenceDelta)
    {
        health.ChangeHealth(rawHealDelta * (1 - _curTolerance / maxTolerance), 0f, true);
        AddTolerance(tolerenceDelta);
    }

    public void AddTolerance(float delta)
    {
        _curTolerance = Mathf.Clamp(_curTolerance + delta, 0, maxTolerance);
    }
}

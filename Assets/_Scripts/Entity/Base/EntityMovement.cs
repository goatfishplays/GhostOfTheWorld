using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    private Coroutine co_LockMovement = null;
    public bool canMove => co_LockMovement == null;
    [SerializeField] protected float forwardMoveSpeed = 9f;
    [SerializeField] protected float sideMoveSpeedMult = 0.75f;
    [SerializeField] protected float backMoveSpeedMult = 0.5f;
    [SerializeField] protected float targetSpeedMult = 1f;
    [SerializeField] protected Dictionary<string, float> targetSpeedMultComponents = new Dictionary<string, float>();

    // compiled speed mult is applied to the final move speed force so will mainly just affect accel and decel I think 
    [SerializeField] protected float compiledSpeedMult = 1f;
    [SerializeField] protected Dictionary<string, float> compiledSpeedMultComponents = new Dictionary<string, float>();
    [SerializeField] protected float accel = 9f;
    [SerializeField] protected float deccel = 9f;
    [SerializeField] protected float velPower = 1f;
    public Rigidbody rb;

    public void SlowDown()
    {
        if (canMove)
        {
            Vector2 speedDiff = -rb.linearVelocity;
            Vector2 movement = Mathf.Pow(speedDiff.magnitude * deccel, velPower) * speedDiff.normalized;
            rb.AddForce(movement * Time.deltaTime);
        }
    }

    // public void SetMoveSpeedMult(float mult)
    // {
    //     targetSpeedMult = mult;
    // }

    /// <summary>
    /// Only call from FixedUpdate
    /// Moves the player based on a relative axis
    /// Can probably make a cheaper version of this to use for non-player entities 
    /// </summary> 
    /// <param name="movementDir"></param>
    public virtual void Move(Vector2 movementDir)
    {
        if (canMove)
        {
            // calculate dir want to move and desired velo
            Vector2 targetSpeed = movementDir.normalized * (forwardMoveSpeed * targetSpeedMult);
            targetSpeed.x *= sideMoveSpeedMult;
            if (movementDir.y < 0)
            {
                targetSpeed.y *= backMoveSpeedMult;
            }
            // change accell depending on situation(if our target target speed wants to not be 0 use decell) 
            // need to split up so don't accidentally use accel for the axis that is supposed to deccel 
            Vector2 accelRate = new Vector2(Mathf.Abs(targetSpeed.x) > .01f ? accel : deccel, Mathf.Abs(targetSpeed.y) > .01f ? accel : deccel);
            // calc diff between current and target  
            Vector3 LocalVelo = transform.InverseTransformDirection(rb.linearVelocity);
            Vector2 speedDif = targetSpeed - new Vector2(LocalVelo.x, LocalVelo.z);
            // applies accel to speed diff, raises to power so accel will increase with higher speeds then applies to desired dir
            Vector3 movement = new Vector3(Mathf.Sign(speedDif.x) * Mathf.Pow(Mathf.Abs(speedDif.x * accelRate.x), velPower) * compiledSpeedMult, 0, Mathf.Sign(speedDif.y) * Mathf.Pow(Mathf.Abs(speedDif.y * accelRate.y), velPower) * compiledSpeedMult);
            // apply force
            // rb.AddForce(movement * Time.deltaTime);
            // rb.AddForce(movement);  
            // Debug.Log(movement);  
            rb.AddRelativeForce(movement);
        }
    }

    #region TargetMovementSpeedMult

    public void RecomputeTargetSpeedMult()
    {
        targetSpeedMult = 1f;
        foreach (float multBoi in targetSpeedMultComponents.Values)
        {
            targetSpeedMult *= multBoi;
        }
    }

    public void AddTimedTargetSpeedMult(float time, string id, float val, bool overridesCurrent = true)
    {
        StartCoroutine(ProcessTimedTarget(time, id, val, overridesCurrent));
    }

    protected IEnumerator ProcessTimedTarget(float time, string id, float val, bool overridesCurrent = true)
    {
        AddTargetSpeedMult(id, val, overridesCurrent);
        yield return new WaitForSeconds(time);
        RemoveTargetSpeedMult(id);
    }

    public void AddTargetSpeedMult(string id, float val, bool overridesCurrent = true)
    {
        if (targetSpeedMultComponents.ContainsKey(id))
        {
            if (overridesCurrent)
            {
                targetSpeedMultComponents[id] = val;
                targetSpeedMult *= val;
            }
            return;
        }
        else
        {
            targetSpeedMultComponents.Add(id, val);
            targetSpeedMult *= val;
        }
    }

    public void RemoveTargetSpeedMult(string id, bool removeID = false)
    {
        if (targetSpeedMultComponents.ContainsKey(id))
        {
            float idSpeed = targetSpeedMultComponents[id];

            if (removeID)
            {
                targetSpeedMultComponents.Remove(id);
            }
            else
            {
                targetSpeedMultComponents[id] = 1f;
            }

            if (idSpeed == 0f)
            {
                RecomputeTargetSpeedMult();
            }
            else
            {
                targetSpeedMult /= idSpeed;
            }

        }
    }

    #endregion TargetMovementSpeedMult

    #region CompiledMovementSpeedMult

    public void RecomputeCompiledSpeedMult()
    {
        compiledSpeedMult = 1f;
        foreach (float multBoi in compiledSpeedMultComponents.Values)
        {
            compiledSpeedMult *= multBoi;
        }
    }

    public void AddTimedCompiledSpeedMult(float time, string id, float val, bool overridesCurrent = true)
    {
        StartCoroutine(ProcessTimedCompiled(time, id, val, overridesCurrent));
    }

    protected IEnumerator ProcessTimedCompiled(float time, string id, float val, bool overridesCurrent = true)
    {
        AddCompiledSpeedMult(id, val, overridesCurrent);
        yield return new WaitForSeconds(time);
        RemoveCompiledSpeedMult(id);
    }

    public void AddCompiledSpeedMult(string id, float val, bool overridesCurrent = true)
    {
        if (compiledSpeedMultComponents.ContainsKey(id))
        {
            if (overridesCurrent)
            {
                compiledSpeedMultComponents[id] = val;
                compiledSpeedMult *= val;
            }
            return;
        }
        else
        {
            compiledSpeedMultComponents.Add(id, val);
            compiledSpeedMult *= val;
        }
    }

    public void RemoveCompiledSpeedMult(string id, bool removeID = false)
    {
        if (compiledSpeedMultComponents.ContainsKey(id))
        {
            float idSpeed = compiledSpeedMultComponents[id];

            if (removeID)
            {
                compiledSpeedMultComponents.Remove(id);
            }
            else
            {
                compiledSpeedMultComponents[id] = 1f;
            }

            if (idSpeed == 0f)
            {
                RecomputeCompiledSpeedMult();
            }
            else
            {
                compiledSpeedMult /= idSpeed;
            }
        }
    }

    #endregion CompiledMovementSpeedMult

    #region MovementLocking

    public virtual void SetLockMovement(float lockTime, bool overridesCurrent = false)
    {
        if (co_LockMovement != null)
        {
            if (overridesCurrent)
            {
                StopCoroutine(co_LockMovement);
            }
            else
            {
                return;
            }
        }
        co_LockMovement = StartCoroutine(LockMovement(lockTime));
    }

    public virtual void UnlockMovement()
    {
        if (co_LockMovement != null)
        {
            StopCoroutine(co_LockMovement);
            co_LockMovement = null;
        }
    }

    protected virtual IEnumerator LockMovement(float lockTime)
    {
        yield return new WaitForSeconds(lockTime);
        co_LockMovement = null;
    }

    #endregion MovementLocking

    public virtual void ApplyKnockback(Vector2 kb)
    {
        rb.AddForce(kb, ForceMode.Impulse);

    }
}

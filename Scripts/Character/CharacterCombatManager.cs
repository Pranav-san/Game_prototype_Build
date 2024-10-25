using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    protected CharacterManager character;

    [Header("Last Attack Animation Performed")]
    public string lastAttackAnimationPerformed;

    [Header("Attack Target")]
    public CharacterManager currentTarget;

    [Header("LockOn Transform")]
    public Transform LockOnTransform;  

    [Header("Attack Type")]
    public AttackType currentAttackType;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();


    }
    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (newTarget != null)
        {
            currentTarget = newTarget;
        }
        else
        {
            currentTarget = null;
        }

    }

    public void OnIsLockedOnChanged(bool old, bool isLoackeOn)
    {
        currentTarget = null;

    }
}

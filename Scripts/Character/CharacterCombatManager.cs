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

    [Header("Attack flags")]
    public bool canBlock = true;
    public bool canBeBackStabbed = true;

    [Header("Critical Attack")]
    [SerializeField] float criticalAttackDistance = 0.7f;
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

    public virtual void AttemptCriticalAttack()
    {

        if(character.isPerformingAction)
            return;
        if (character.characterStatsManager.currentStamina<=0)
            return;

        RaycastHit[] hits = Physics.RaycastAll(character.characterCombatManager.LockOnTransform.position,
         character.transform.TransformDirection(Vector3.forward), criticalAttackDistance, WorldUtilityManager.Instance.GetCharacterLayer());



    }

    public void OnIsLockedOnChanged(bool old, bool isLoackeOn)
    {
        currentTarget = null;

    }

    public void EnableIsInvulnerable()
    {
        character.isInvulnerable = true;

    }

    public void DisableIsInvulnerable()
    {
        character.isInvulnerable = false;

    }
}

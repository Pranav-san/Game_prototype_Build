using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string hit_Forward_Medium_01= "Damage_01";
    public string hit_Backward_Medium_01 = "Damage_04";
    public string hit_Left_Medium_01= "Damage_02";
    public string hit_Right_Medium_01= "Damage_03";

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");

    }
    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontalAmount;
        float snappedVerticalAmount;

        if(horizontalMovement > 0.0f &&  horizontalMovement<=0.5f)
        {
            snappedHorizontalAmount = 0.5f;
        }
        else if(horizontalMovement> 0.5f&& horizontalMovement<=1)
        {
            snappedHorizontalAmount =1f;

        }
        else if (horizontalMovement<0.0f &&  horizontalMovement>= -0.5f)
        {
            snappedHorizontalAmount = -0.5f;

        }
        else if (horizontalMovement < -0.5f&& horizontalMovement >=-1)
        {
            snappedHorizontalAmount =-1f;

        }
        else
        {
            snappedHorizontalAmount = 0f;
        }
        if (verticalMovement > 0.0f && verticalMovement<=0.5f)
        {
            snappedVerticalAmount = 0.5f;
        }
        else if (verticalMovement> 0.5f&& verticalMovement<=1)
        {
            snappedVerticalAmount =1f;

        }
        else if (verticalMovement<0.0f &&  verticalMovement>= -0.5f)
        {
            snappedVerticalAmount = -0.5f;

        }
        else if (verticalMovement < -0.5f&& verticalMovement >=-1)
        {
            snappedVerticalAmount =-1f;

        }
        else
        {
            snappedVerticalAmount = 0f;
        }

        if (isSprinting)
        {
            snappedVerticalAmount = 2;

        }
        character.animator.SetFloat(horizontal, snappedHorizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, snappedVerticalAmount, 0.1f, Time.deltaTime);

    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAction, 
        bool applyRootMotion = true, 
        bool canRotate= false, 
        bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        
        //can be used to stop character from attempting new Actions
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;    

    }

    public virtual void PlayTargetAttackActionAnimation( AttackType attackType,
       string targetAnimation,
       bool isPerformingAction,
       bool applyRootMotion = true,
       bool canRotate = false,
       bool canMove = false)
    {
        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed=targetAnimation;
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);

        //can be used to stop character from attempting new Actions
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;

    }

    public virtual  void EnableCanDoCombo()
    {
        
    }

    public virtual void DisableCanDoCombo()
    {

    }

}

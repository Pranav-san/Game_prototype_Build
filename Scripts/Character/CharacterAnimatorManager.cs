using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;
   

    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string lastAnimationPlayed;


    //Ping Hit Reactions
    public string hit_Forward_Ping_01 = "Damage_01";
    public string hit_Forward_Ping_02 = "Damage_01";

    public string hit_Backward_Ping_01 = "Damage_04";
    public string hit_Backward_Ping_02 = "Damage_04";

    public string hit_Left_Ping_01 = "Damage_02";
    public string hit_Left_Ping_02 = "Damage_02";

    public string hit_Right_Ping_01 = "Damage_03";
    public string hit_Right_Ping_02 = "Damage_03";

    public List<string> forward_Ping_Damage = new List<string>();
    public List<string> backward_Ping_Damage = new List<string>();
    public List<string> left_Ping_Damage = new List<string>();
    public List<string> right_Ping_Damage = new List<string>();




    //Medium Hit Reactions
    public string hit_Forward_Medium_01= "Damage_01";
    //public string hit_Forward_Medium_02= "Damage_01";

    public string hit_Backward_Medium_01 = "Damage_04";
    //public string hit_Backward_Medium_02 = "Damage_04";

    public string hit_Left_Medium_01= "Damage_02";
    //public string hit_Left_Medium_02= "Damage_02";

    public string hit_Right_Medium_01= "Damage_03";
    //public string hit_Right_Medium_02= "Damage_03";

    public List<string> forward_Medium_Damage = new List<string>();
    public List<string> backward_Medium_Damage = new List<string>();
    public List<string> left_Medium_Damage = new List<string>();
    public List<string> right_Medium_Damage = new List<string>();


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
       
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");

    }

    protected virtual void Start()
    {
        //Ping Hit Reaction List
        forward_Ping_Damage.Add(hit_Forward_Ping_01);
        forward_Ping_Damage.Add(hit_Forward_Ping_02);

        backward_Ping_Damage.Add(hit_Backward_Ping_01);
        backward_Ping_Damage.Add(hit_Backward_Ping_02);

        left_Ping_Damage.Add(hit_Left_Ping_01);
        left_Ping_Damage.Add(hit_Left_Ping_02);

        right_Ping_Damage.Add(hit_Right_Ping_01);
        right_Ping_Damage.Add(hit_Right_Ping_02);



        //Medium Hit Reaction List
        forward_Medium_Damage.Add(hit_Forward_Medium_01);
        //forward_Medium_Damage.Add(hit_Forward_Medium_02);

        backward_Medium_Damage.Add(hit_Backward_Medium_01);
        //backward_Medium_Damage.Add(hit_Backward_Medium_02);

        left_Medium_Damage.Add(hit_Left_Medium_01);
        //left_Medium_Damage.Add(hit_Left_Medium_02);

        right_Medium_Damage.Add(hit_Right_Medium_01);
        //right_Medium_Damage.Add(hit_Right_Medium_02);
    }
   
    


    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting, bool isAiming)
    {
        float snappedHorizontalAmount;
        float snappedVerticalAmount;

        if(character==null)
            return;

        if (isAiming)
        {
            if (horizontalMovement > 0.0f &&  horizontalMovement<=1 && isAiming)
            {
                snappedHorizontalAmount = 0.5f;
            }
            else if (horizontalMovement<0.0f &&  horizontalMovement>=-1 &&isAiming)
            {
                snappedHorizontalAmount = -0.5f;

            }
            else
            {
                snappedHorizontalAmount = 0f;
            }
            if (verticalMovement > 0.0f && verticalMovement<=1 && isAiming)
            {
                snappedVerticalAmount = 0.5f;
            }
            else if (verticalMovement<0.0f &&  verticalMovement>=-1 && isAiming)
            {
                snappedVerticalAmount = -0.5f;

            }
            else
            {
                snappedVerticalAmount =0f;
            }

            character.animator.SetFloat(horizontal, snappedHorizontalAmount, 0.2f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVerticalAmount, 0.2f, Time.deltaTime);

        }





        if (!isAiming && !PlayerCamera.instance.player.playerCombatManager.isLockedOn)
        {
            if (horizontalMovement > 0.0f &&  horizontalMovement<=0.5f)
            {
                snappedHorizontalAmount = 0.5f;
            }
            else if (horizontalMovement> 0.5f&& horizontalMovement<=1)
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
                snappedVerticalAmount =2f;

            }
            else if (verticalMovement<0.0f &&  verticalMovement>= -0.5f)
            {
                snappedVerticalAmount = -0.5f;

            }
            else if (verticalMovement < -0.5f&& verticalMovement >=-1)
            {
                snappedVerticalAmount =-2f;

            }
            else
            {
                snappedVerticalAmount = 0f;
            }

            if (isSprinting)
            {
                snappedVerticalAmount = 2.5f;

            }

            character.animator.SetFloat(horizontal, snappedHorizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVerticalAmount, 0.1f, Time.deltaTime);

        }

        if (PlayerCamera.instance.player.playerCombatManager.isLockedOn)
        {
            // Use raw input for smooth strafe blending
            snappedHorizontalAmount = horizontalMovement;
            snappedVerticalAmount = verticalMovement;

            // Optional: mild smoothing
            character.animator.SetFloat(horizontal, snappedHorizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVerticalAmount, 0.1f, Time.deltaTime);
            
        }












    }

    public string GetRandomAnimationsFromList(List<string> AnimationList)
    {
        List<string> finalList = new List<string>();
        foreach(var item in AnimationList)
        {
            finalList.Add(item);
        }
        //Check if Already Played This Damage Animation So It Doesn't Repeat
        //finalList.Remove(lastAnimationPlayed);

        //Check The List For null Entries And Then Remove It
        for(int i = finalList.Count-1; i > -1; i--)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }

        }
        int randomValue = Random.Range(0, finalList.Count);

        return finalList[randomValue];
    }


    public void SetAnimatorMovementParameters(float horizontalMovement, float verticalMovement)
    {
        character.animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
        character.animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false,
        bool canrun = true,
        bool canRoll = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        
        //can be used to stop character from attempting new Actions
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;
        character.canRun = canrun;
        character.canRoll = canRoll;

    }

    public virtual void PlayTargetActionAnimationInstantly(
        string targetAnimation,
        bool isPerformingAction,
        bool applyRootMotion = true,
        bool canRotate = false,
        bool canMove = false,
        bool canrun = true,
        bool canRoll = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.Play(targetAnimation);

        //can be used to stop character from attempting new Actions
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;
        character.canRun = canrun;
        character.canRoll = canRoll;

    }
    public virtual void PlayTargetAttackActionAnimation(WeaponItem weapon, 
        AttackType attackType,
       string targetAnimation,
       bool isPerformingAction,
       bool applyRootMotion = true,
       bool canRotate = false,
       bool canMove = false,
       bool canRoll = false)
    {

        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed=targetAnimation;
        UpdateAnimatorController(weapon.weaponAnimator);
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);

        //can be used to stop character from attempting new Actions
        character.isPerformingAction = isPerformingAction;
        character.canRoll = canRoll;
        character.canRotate = canRotate;
        character.canMove = canMove;

    }


    public virtual void PlayOffHandAttackActionAnimation(WeaponItem weapon,
       AttackType attackType,
      string targetAnimation,
      bool isPerformingAction,
      bool applyRootMotion = true,
      bool canRotate = false,
      bool canMove = false,
      bool canRoll = false)
    {

        character.characterCombatManager.currentAttackType = attackType;
        character.characterCombatManager.lastAttackAnimationPerformed=targetAnimation;
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);


        //can be used to stop character from attempting new Actions
        character.isPerformingAction = isPerformingAction;
        character.canRoll = canRoll;
        character.canRotate = canRotate;
        character.canMove = canMove;

    }



    public virtual  void EnableCanDoCombo()
    {




        
    }

    public virtual void DisableCanDoCombo()
    {




    }

    public void UpdateAnimatorController(AnimatorOverrideController weaponController)
    {
        if (character.isPerformingAction)
            return;

        character.animator.runtimeAnimatorController = weaponController;

    }



}

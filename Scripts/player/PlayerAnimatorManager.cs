using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    playerManager player;

    [Header("Hand Ik Constraints ")]
    public TwoBoneIKConstraint rightHandIk;
    public TwoBoneIKConstraint lefttHandIk;
    public RigLayer handriglayer;

    [Header("Aim Constraints")]
    public MultiAimConstraint spine_01;
    public MultiAimConstraint spine_02;
    public MultiAimConstraint spine_03;
    public MultiAimConstraint head;
    public MultiAimConstraint rightHand;
    public MultiAimConstraint lefttHand;

    [SerializeField] public  RigBuilder rig;


    


    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<playerManager>();
        rig = GetComponent<RigBuilder>();
    }

    private void LateUpdate()
    {
        if (player.isReloadedWeapon && player.isAiming)
        {
            OnReloadEnd();


        }

        //else if (player.isExitingToEmptyAfterReload && !player.isAiming)
        //{
        //    rightHandIk.weight = 0f;
        //    lefttHandIk.weight = 0f;
        //    player.isExitingToEmptyAfterReload = false;
        //}

       

    }
    private void OnAnimatorMove()
    {
        if (!player.characterController.enabled)
        {
            player.animator.ApplyBuiltinRootMotion();
        }
        if (player.applyRootMotion)
        {
            player.canMove=false;
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
           
            
        }
    }


  



    public override void EnableCanDoCombo()
    {
        if (player.isUsingRightHand)
        {
            player.playerCombatManager.canComboWithMainHandleWeapon = true;
            player.isUsingRightHand = true;
        }
        else
        {
            //Enable Off Hand Weapon
            player.playerCombatManager.canComboWithOffHandleWeapon = true;
            player.isUsingLeftHand = true;
        }
    }

    public override void DisableCanDoCombo()
    {
        player.playerCombatManager.canComboWithMainHandleWeapon = false;
        player.playerCombatManager.canComboWithOffHandleWeapon = false;


    }

    public void AssignHandIK(RightHandIKTarget rightHandIKTarget, LeftHandIKTarget leftHandIKTarget)
    {
        rig.enabled = false;
        if(rightHandIKTarget != null && leftHandIKTarget!=null)
        {
            rightHandIk.data.target = rightHandIKTarget.transform;
            lefttHandIk.data.target = leftHandIKTarget.transform;

            rig.enabled = true;
            rig.Build();

        }
        else
        {
            rightHandIk.data.target =null;
            lefttHandIk.data.target = null;

            rig.enabled = true;
            rig.Build();

        }
       
    }

    public void EnableDisableIK(float rightHandIKWeight, float leftHandIKWeight)
    {

        rightHandIk.weight = rightHandIKWeight;
        lefttHandIk.weight = leftHandIKWeight;
       
    }

    public void DisableIK()
    {
        rig.enabled =false;
        rightHandIk.data.target =null;
        lefttHandIk.data.target=null;

        rig.Clear();
    }

   

    public void UpdateAimedConstraints () 
    {
        if (player.isAiming)
        {
            spine_01.weight = 0.5f;
            spine_02.weight = 0.75f;
            spine_03.weight = 1f;
            head.weight = 1f;
            rightHand.weight= 0.9f;
            

            
        }
        else
        {

            spine_01.weight = 0f;
            spine_02.weight = 0f;
            spine_03.weight = 0f;
            head.weight = 0;
            rightHand.weight= 0f;
           

        }

    }


    public void UpdateAimedConstraintsForBow()
    {
        if (player.isAiming)
        {
            spine_01.weight = 1f;
            spine_02.weight = 1f;
            head.weight = 0f;
            rightHand.weight= 0f;
            lefttHand.weight= 0f;



        }
        else
        {

            spine_01.weight = 0f;
            spine_02.weight = 0f;
            head.weight = 0;
            rightHand.weight= 0f;
            lefttHand.weight= 0f;


        }

    }

    public void OnReloadEnd()
    {
        rightHandIk.weight = 1f;
        lefttHandIk.weight = 1f;
        player.isReloadedWeapon = false;    
    }

    public void EnableIKAfterReload()
    {
        player.isReloadedWeapon = true;

    }


}

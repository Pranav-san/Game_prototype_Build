using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{


    [SerializeField] playerManager player;
    [SerializeField] public bool isLockedOn = false;

    public ProjectileSlot currentProjectileBeingUsed;


    public WeaponItem currentWeaponBeingUsed;

    [Header("Flags")]
    public bool canComboWithMainHandleWeapon = false;
    //public bool canComboWithOffHandleWeapon = false;

    protected override void Awake()
    {
        base.Awake();
        player= GetComponent<playerManager>();

       
        
    }


    public void PerformWeaponBasedAction(WeaponItemBasedAction weaponAction, WeaponItem WeaponPerformiingAction)
    {
        //if (weaponAction == null || WeaponPerformiingAction == null)
        //{
        //    Debug.LogError("One of the parameters is null.");
        //    return;
        //}

        weaponAction.AttemptToPerformAction(player, WeaponPerformiingAction);
    }

    public virtual void DrainStaminaBasedOnAttack()
    {
        float staminaDeducted = 0;

        if (currentWeaponBeingUsed == null)
        {
            return;
        }

        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost*currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;

            default:
                break;
        }

        player.characterStatsManager.ConsumeStamina(staminaDeducted);
        player.isUsingRightHand = true;
        player.isUsingLeftHand = false;


    }

    public void InstantiateSpellWarmUpFX()
    {
        if(player.playerInventoryManager.currentSpell==null)
            return;
        player.playerInventoryManager.currentSpell.InstantiateWarmUpSpellFX(player);

    }

    public void InstantiateSpellReleaseFX()
    {
        if (player.playerInventoryManager.currentSpell==null)
            return;

        player.playerInventoryManager.currentSpell.InstantiateReleaseFX(player);

    }

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        newTarget.characterStatsManager.uI_Character_HP_Bar.lockOnUI.SetActive(true);

        PlayerCamera.instance.SetLockCameraHeight();
    }

    public void CheckIfTargetIsDead(CharacterManager targetToCheck)
    {
        if (currentTarget.characterStatsManager.isDead)
        {
            currentTarget = null;
            isLockedOn = false;
            PlayerCamera.instance.ClearLockOnTargets();
            

            Debug.Log("Unlocked From Dead Target");
        }
    }

    public void ReleaseArrow()
    {

        player.playerInventoryManager.hasArrowNotched = false;

        if(player.playerEquipmentManager.notchedArrow != null)
            Destroy(player.playerEquipmentManager.notchedArrow );

        

        Animator bowAnimator;

        bowAnimator = player.playerInventoryManager.currentRightHandWeapon.weaponModel.GetComponentInChildren<Animator>();

        if (bowAnimator == null)
            return;





        //Animate The Bow
        //Play Fire Animation
        bowAnimator.SetBool("isDrawn", false);
        bowAnimator.Play("Bow_Fire");
       player.canMove = false;

        //Projectile we are Firing
        RangedProjectileItem projectileItem = null;

        switch(currentProjectileBeingUsed)
        {
            case ProjectileSlot.Main:
                projectileItem = player.playerInventoryManager.mainProjectile;
                break;
            case ProjectileSlot.Secondary:
                projectileItem = player.playerInventoryManager.secondaryProjectile;
                break;
                default:
                break;
        }

        if (projectileItem == null)
            return;

        if(projectileItem.currentAmmoAmount <=0 )
            return;

        Transform projectileInstantiationLocation;
        GameObject projectileGameObject;
        Rigidbody projectileRigidbody;
        RangedProjectileDamageCollider projectileDamageCollider;

        //Subtract Ammo
        projectileItem.currentAmmoAmount -= 1;

        projectileInstantiationLocation = player.playerCombatManager.LockOnTransform;
        projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation);
        projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
        projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

        //Formula To Set Damage
        projectileDamageCollider.physicalDamage = 20;
        projectileDamageCollider.characterShootingProjectile = player;

        //Fire Arrow Based On On out of 3 Variations

        
        if (player.playerCombatManager.currentTarget != null)
        {
            Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position
                - projectileGameObject.transform.position);

            projectileGameObject.transform.rotation = arrowRotation;

            
        }


            //Get All Character Damage Collider and Ignore Self
            
        Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
        List<Collider> collidersArrowWillIgnore = new List<Collider>();
            
        foreach (var item in characterColliders)
                collidersArrowWillIgnore.Add(item);
            
        foreach (Collider hitBox in collidersArrowWillIgnore)
          Physics.IgnoreCollision(projectileDamageCollider.damageCollider, hitBox, true);
         
        projectileRigidbody.AddForce(projectileGameObject.transform.forward* projectileItem.forwardVelocity);
        projectileGameObject.transform.parent = null;




        






    }



}

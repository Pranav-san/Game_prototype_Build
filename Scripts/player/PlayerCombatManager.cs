using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{


    [SerializeField] playerManager player;
    [SerializeField] public bool isLockedOn = false;


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
        if (weaponAction == null || WeaponPerformiingAction == null)
        {
            Debug.LogError("One of the parameters is null.");
            return;
        }

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

    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        PlayerCamera.instance.SetLockCameraHeight();
    }



}

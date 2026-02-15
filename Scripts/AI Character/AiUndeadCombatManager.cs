using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiUndeadCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField]ManualDamageCollider rightDamageCollider;
    [SerializeField]ManualDamageCollider leftDamageCollider;

    [Header("Grapple Colliders")]
    [SerializeField] ZombieGrappleCollider rightGrappleCollider;
    [SerializeField] ZombieGrappleCollider leftGrappleCollider;

    [Header("Explosion Colliders")]
    //[SerializeField]ExploderDamageCollider rightExploderCollider;
    [SerializeField]ExploderDamageCollider lefttExploderCollider;


    [Header("Damage Modifiers")]
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;

    public void SetAttack01Damage()
    {
        rightDamageCollider.physicalDamage = baseDamage*attack01DamageModifier;
        leftDamageCollider.physicalDamage = baseDamage*attack01DamageModifier*attack02DamageModifier;

        rightDamageCollider.poiseDamage = basePoiseDamage*attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        rightDamageCollider.physicalDamage = baseDamage*attack02DamageModifier;
        leftDamageCollider.physicalDamage = baseDamage*attack02DamageModifier*attack02DamageModifier;

        rightDamageCollider.poiseDamage = basePoiseDamage*attack02DamageModifier;
    }

    public void SetExpolderDamage()
    {
       
        lefttExploderCollider.physicalDamage = baseDamage*attack01DamageModifier*attack02DamageModifier;

        lefttExploderCollider.poiseDamage = basePoiseDamage*attack01DamageModifier;
    }

    public void OpenRightHandDamageCollider()
    {
        rightDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandDamageCollider()
    {
        leftDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftDamageCollider.DisableDamageCollider();
    }

    public void OpenLeftHandGrappleCollider()
    {
        leftGrappleCollider.EnableDamageCollider();
    }

    public void OpenRightHandGrappleCollider()
    {
        rightGrappleCollider.EnableDamageCollider();
    }

    public void CloseRightHandGrappleCollider()
    {
        rightGrappleCollider.DisableDamageCollider();

    }

    public void CloseLeftHandGrappleCollider()
    {
        leftGrappleCollider.DisableDamageCollider();

    }

    public void OpenGrappleColliders()
    {
        leftGrappleCollider.EnableDamageCollider();
        rightGrappleCollider.EnableDamageCollider();

    }
    public void CloseGrappleColliders()
    {
        leftGrappleCollider.DisableDamageCollider();
        rightGrappleCollider.DisableDamageCollider();
    }

    public void OpenExplosionColliders()
    {
       lefttExploderCollider.EnableDamageCollider();
     

    }

    public void CloseExplosionleColliders()
    {
        lefttExploderCollider.DisableDamageCollider();
       
       
    }

    public void ExpolderDeathEvent()
    {
        aiCharacter.characterStatsManager.currentHealth = 0;
        lefttExploderCollider.explosionVfx.SetActive(false);
        PlayerInputManager.Instance.player.playerCombatManager.isLockedOn = false;
        PlayerInputManager.Instance.player.playerCombatManager.currentTarget = null;
        //PlayerCamera.instance.SetLockCameraHeight();

    }

    protected override void DisableAllDamageColliders()
    {
        CloseRightHandDamageCollider();
        CloseLeftHandDamageCollider();
        //CloseGrappleColliders();
        //CloseExplosionleColliders();
    }











}

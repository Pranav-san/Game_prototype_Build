using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{


    [SerializeField] playerManager player;
    [SerializeField] public bool isLockedOn = false;
    [SerializeField] public bool isAimLockedOn = false;

    public ProjectileSlot currentProjectileBeingUsed;


    public WeaponItem currentWeaponBeingUsed;

    [HideInInspector] public Transform softTarget = null;

    [Header("Projectile")]
    Vector3 projectileAimDirection;

    [Header("Not LockedON Attack Rotation Setting ")]
    [Range(0, 180)] public float angleLimit = 180f;

    [Header("Flags")]
    public bool canComboWithMainHandleWeapon = false;
    public bool canComboWithOffHandleWeapon = false;
    public bool isUsingItem = false;

    [Header("Rotate and Face The Target When Performing Attack")]
   [SerializeField] float lockedOnAttackRotationRadius = 2.5f;

    protected override void Awake()
    {
        base.Awake();
        player= GetComponent<playerManager>();



    }

    private void LateUpdate()
    {
        if (!isLockedOn && softTarget != null && player.isAttacking)
        {
            // If softTarget is destroyed
            if (softTarget == null)
            {
                softTarget = null;
                return;
            }

            // If softTarget is dead
            if (softTarget.TryGetComponent<CharacterStatsManager>(out var stats))
            {
                if (stats.isDead)
                {
                    softTarget = null;
                    return;
                }
            }

            // Face the target instantly
            RotateTowardsTarget(softTarget);
        }

        //  Clear softTarget when attack ends
        if (!player.isAttacking)
        {
            softTarget = null;
        }
    }


    public void PerformWeaponBasedAction(WeaponItemBasedAction weaponAction, WeaponItem WeaponPerformiingAction)
    {
        if (weaponAction == null || WeaponPerformiingAction == null)
        {
            Debug.LogError("One of the parameters is null.");
            return;
        }

        if (!isLockedOn)
        {
            TryAutoFaceNearestEnemy(3f, angleLimit);
        }
        else
        {
            if (currentTarget!=null)
            {
                RotateTowardsLockedOnTargetWhenPerformingAnAttack();
            }
        }

        weaponAction.AttemptToPerformAction(player, WeaponPerformiingAction);
    }

    public override void AttemptRiposte(RaycastHit hit)
    {
      

        CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

        if (targetCharacter == null)
            return;

        if (!targetCharacter.characterCombatManager.isRipostable)
            return;

        if(!canBeRiposted)
            return;

        MeleeWeaponItem riposteWeapon;
        MeleeWeaponDamageCollider riposteCollider;

        riposteWeapon =  player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
        riposteCollider= player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;

        StartCoroutine(character.characterCombatManager.ForceMoveEnemyCharacterToRipostePosition(targetCharacter, WorldUtilityManager.Instance.GetRipostingPositionBasedOnWeaponClass(riposteWeapon.weaponClass)));


        character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Riposte_01", true);

       
        TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);


        damageEffect.physicalDamage = riposteCollider.physicalDamage;
        damageEffect.firelDamage = riposteCollider.firelDamage;
        damageEffect.magicDamage = riposteCollider.magicDamage;
        damageEffect.lightininglDamage = riposteCollider.lightininglDamage;

        damageEffect.physicalDamage *= riposteWeapon.riposte_Attack_01_Modifier;
        damageEffect.firelDamage *= riposteWeapon.riposte_Attack_01_Modifier;
        damageEffect.magicDamage *= riposteWeapon.riposte_Attack_01_Modifier;
        damageEffect.lightininglDamage *= riposteWeapon.riposte_Attack_01_Modifier;

        targetCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("Riposted_01", true);

    }


    public override void AttemptBackStab(RaycastHit hit)
    {


        CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

        if (targetCharacter == null)
            return;

        if(player.isPerformingCriticalAttack)
            return;

        if(!canBeBackStabed)
            return;

        MeleeWeaponItem backstabWeapon;
        MeleeWeaponDamageCollider backstabCollider;

        backstabWeapon =  player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
        backstabCollider= player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;

        StartCoroutine(character.characterCombatManager.ForceMoveEnemyCharacterToBackStabPosition(targetCharacter, WorldUtilityManager.Instance.GetBackStabPositionBasedOnWeaponClass(backstabWeapon.weaponClass)));


        character.characterAnimatorManager.PlayTargetActionAnimationInstantly("BackStab_01", true);


        TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);


        damageEffect.physicalDamage = backstabCollider.physicalDamage;
        damageEffect.firelDamage = backstabCollider.firelDamage;
        damageEffect.magicDamage = backstabCollider.magicDamage;
        damageEffect.lightininglDamage = backstabCollider.lightininglDamage;

        damageEffect.physicalDamage *= backstabWeapon.backstab_Attack_01_Modifier;
        damageEffect.firelDamage *= backstabWeapon.backstab_Attack_01_Modifier;
        damageEffect.magicDamage *= backstabWeapon.backstab_Attack_01_Modifier;
        damageEffect.lightininglDamage *= backstabWeapon.backstab_Attack_01_Modifier;

        targetCharacter.characterAnimatorManager.PlayTargetActionAnimationInstantly("BackStabbed_01", true);

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
            case AttackType.LightAttack02:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost*currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;

            case AttackType.HeavyAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost*currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                break;


            default:
                break;
        }

        player.characterStatsManager.ConsumeStamina(staminaDeducted);
        


    }

    public void InstantiateSpellWarmUpFX()
    {
        if (player.playerInventoryManager.currentSpell==null)
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

    private void TryAutoFaceNearestEnemy(float radius = 3f, float angleLimit = 180f)
    {
        if (isLockedOn) return;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, radius, WorldUtilityManager.Instance.GetCharacterLayer());

        Transform nearest = null;
        float smallestAngle = angleLimit;

        foreach (var col in colliders)
        {
            // Skip self
            if (col.transform.root == transform.root)
                continue;

            // Get enemy character manager
            CharacterManager potentialTarget = col.GetComponent<CharacterManager>();
            if (potentialTarget == null || potentialTarget.characterStatsManager == null || potentialTarget.characterStatsManager.isDead)
                continue;

            // Angle check
            Vector3 directionToTarget = (potentialTarget.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            if (angle < smallestAngle)
            {
                smallestAngle = angle;
                nearest = potentialTarget.transform;
            }
        }

        if (nearest != null)
        {
            softTarget = nearest;
            RotateTowardsTarget(softTarget); // instantly face target
        }
    }

    private void RotateTowardsLockedOnTargetWhenPerformingAnAttack()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distanceToTarget <= lockedOnAttackRotationRadius)
        {
            RotateTowardsTarget(currentTarget.characterCombatManager.LockOnTransform);
        }


    }




    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }


    public void ReleaseArrow()
    {

        player.hasArrowNotched = false;

        if (player.playerEquipmentManager.notchedArrow != null)
            Destroy(player.playerEquipmentManager.notchedArrow);



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

        switch (currentProjectileBeingUsed)
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

        if (projectileItem.currentAmmoAmount <=0)
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

        //Aiming
        if (player.isAiming)
        {
            // Cast a short ray from camera center
            Ray ray = new Ray(PlayerCamera.instance.cameraObject.transform.position, PlayerCamera.instance.cameraObject.transform.forward);

            RaycastHit hit;

            //  Reduce raycast distance (1000f is overkill)
            float rayDistance = 20f;

            //  Use specific layers to avoid unnecessary hits
            int layerMask = LayerMask.GetMask("Default", "Damageable Character"); // Replace as needed

#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green, 20);
#endif

            if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                projectileAimDirection = hit.point;
            }
            else
            {
                projectileAimDirection = ray.GetPoint(rayDistance);
            }

            projectileGameObject.transform.LookAt(projectileAimDirection);
            projectileGameObject.transform.position += projectileGameObject.transform.forward * 0.2f;


        }
        else
        {
            //Locked And Not Aiming
            if (player.playerCombatManager.currentTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position
                    - projectileGameObject.transform.position);

                projectileGameObject.transform.rotation = arrowRotation;


            }
            //unlocked And Not Aiming
            else
            {
                Quaternion arrowRotation = Quaternion.LookRotation(player.transform.forward);
                projectileGameObject.transform.rotation = arrowRotation;

            }

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

    public void FireBullet()
    {
        if(player.playerInventoryManager.FireBullet)
        {
            // Projectile we are firing
            RangedProjectileItem projectileItem = null;

            switch (currentProjectileBeingUsed)
            {
                case ProjectileSlot.Main:
                    projectileItem = player.playerInventoryManager.mainProjectile;
                    break;

                default:
                    break;

            }

            if (projectileItem == null)
                return;

            if (projectileItem.currentMagazineAmmo <= 0)
            {
              
                return;
            }



            // Subtract ammo
            projectileItem.currentMagazineAmmo -= 1;

            // Instantiate projectile
            Transform projectileInstantiationLocation = player.playerEquipmentManager.projectileInstantiationTransform;
            GameObject projectileGameObject = Instantiate(projectileItem.releaseProjectileModel, projectileInstantiationLocation.position, projectileInstantiationLocation.rotation);
            RangedProjectileDamageCollider projectileDamageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();
            Rigidbody projectileRigidbody = projectileGameObject.GetComponent<Rigidbody>();

            // Set damage (replace 20 with weapon damage if needed)
            projectileDamageCollider.physicalDamage = 20;
            projectileDamageCollider.characterShootingProjectile = player;

            // Aiming logic — same as bow
            if (player.isAiming)
            {
                // Cast a short ray from camera center
                Ray ray = new Ray(PlayerCamera.instance.cameraObject.transform.position, PlayerCamera.instance.cameraObject.transform.forward);

                RaycastHit hit;

                //  Reduce raycast distance (1000f is overkill)
                float rayDistance = 20f;

                //  Use specific layers to avoid unnecessary hits
                int layerMask = LayerMask.GetMask("Default", "Damageable Character"); // Replace as needed

#if UNITY_EDITOR
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green, 20);
#endif

                if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
                {
                    projectileAimDirection = hit.point;
                }
                else
                {
                    projectileAimDirection = ray.GetPoint(rayDistance);
                }

                projectileGameObject.transform.LookAt(projectileAimDirection);
                projectileGameObject.transform.position += projectileGameObject.transform.forward * 0.2f;


            }
            else
            {
                //Locked And Not Aiming
                if (player.playerCombatManager.currentTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position
                        - projectileGameObject.transform.position);

                    projectileGameObject.transform.rotation = arrowRotation;


                }
                //unlocked And Not Aiming
                else
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.transform.forward);
                    projectileGameObject.transform.rotation = arrowRotation;

                }
            }

            //Play Sfx
            player.characterSoundFxManager.PlayShotgunSfx();

            // Ignore self colliders
            Collider[] characterColliders = player.GetComponentsInChildren<Collider>();
            foreach (var col in characterColliders)
                Physics.IgnoreCollision(projectileDamageCollider.damageCollider, col, true);

            // Fire projectile
            projectileRigidbody.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
            projectileGameObject.transform.parent = null;


        }











        // Optional: play muzzle flash / SFX here

    }

    public void ResetFireBulletFlag()
    {
        player.playerInventoryManager.FireBullet = false;
        player.animator.SetBool("FireBullet", false);
    }

    public void ReloadWeapon(RangedProjectileItem projectileItem)
    {
        projectileItem = player.playerInventoryManager.mainProjectile;
        if (projectileItem == null)
        {
            Debug.Log("Projectile Item missing");
            return;

        }
           
        if (projectileItem.currentAmmoAmount <= 0)
            return; // No reserve ammo

        int needed = projectileItem.magazineSize - projectileItem.currentMagazineAmmo;
        int ammoToLoad = Mathf.Min(needed, projectileItem.currentAmmoAmount);

        projectileItem.currentMagazineAmmo += ammoToLoad;
        projectileItem.currentAmmoAmount -= ammoToLoad;

        Debug.Log("Reloaded");

        // Optionally trigger reload animation
       

    }

    public void EnableIkAfterReaload()
    {
        player.playerAnimatorManager.EnableDisableIK(1, 1);
        Debug.Log("IK Enabled After reload");
    }

   

    public void SuccessfullyUsedQuickSlotitem()
    {
        if(player.playerInventoryManager.currentQuickSlotItem != null)
        player.playerInventoryManager.currentQuickSlotItem.SuccessfullyUsedItem(player);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f); // radius must match your OverlapSphere

        // Optional: Draw a forward-facing arc (cone)
        Gizmos.color = Color.yellow;
        Vector3 forward = transform.forward * 2f; // length of the cone
        Quaternion leftRayRotation = Quaternion.AngleAxis(-angleLimit / 2, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(angleLimit / 2, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.DrawRay(transform.position, leftRayDirection);
        Gizmos.DrawRay(transform.position, rightRayDirection);
    }






}

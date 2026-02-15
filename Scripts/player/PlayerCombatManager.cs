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

    [Header("Not LockedOn Attack Rotation Setting ")]
    [Range(0, 180)] public float angleLimit = 180f;

    [Header("Flags")]
    public bool canComboWithMainHandleWeapon = false;
    public bool canComboWithOffHandleWeapon = false;
    public bool isUsingItem = false;

    [Header("Rotate and Face The Target When Performing Attack")]
   [SerializeField] float lockedOnAttackRotationRadius = 2.5f;

    [Header("Last DeadSpot")]
    public GameObject lastDeadSpot;

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

        if (!isLockedOn && !player.isUsingLeftHand)
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

    //Create Dead Spot 
    public void CreateDeadSpot(Vector3 position, int runeCount, bool removePlayersRunes = true)
    {
        if (lastDeadSpot==null)
        {
            //Spwan The Dead Spot VFX
            GameObject deadSpotFX = Instantiate(WorldCharacterEffectsManager.instance.deadSpotVFX);
            lastDeadSpot = deadSpotFX;  

            //Set Its World Position
            deadSpotFX.transform.position = position;

            //Set The Rune Count
            PickUpRunesInteractable pickUpRunes = deadSpotFX.GetComponent<PickUpRunesInteractable>();
            pickUpRunes.runes = runeCount;
            if (removePlayersRunes)
                player.playerStatsManager.AddRunes(-player.playerStatsManager.runes);

            WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot = true;
            WorldSaveGameManager.instance.currentCharacterData.deadSpotRuneCount = pickUpRunes.runes;
            WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionX = position.x;
            WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionY = position.y;
            WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionZ = position.z;

        }
        else
        {
            Destroy(lastDeadSpot);

            //Spwan The Dead Spot VFX
            GameObject deadSpotFX = Instantiate(WorldCharacterEffectsManager.instance.deadSpotVFX);
            lastDeadSpot = deadSpotFX;

            //Set Its World Position
            deadSpotFX.transform.position = position;

            //Set The Rune Count
            PickUpRunesInteractable pickUpRunes = deadSpotFX.GetComponent<PickUpRunesInteractable>();
            pickUpRunes.runes = runeCount;
            if (removePlayersRunes)
                player.playerStatsManager.AddRunes(-player.playerStatsManager.runes);

            WorldSaveGameManager.instance.currentCharacterData.hasDeadSpot = true;
            WorldSaveGameManager.instance.currentCharacterData.deadSpotRuneCount = pickUpRunes.runes;
            WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionX = position.x;
            WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionY = position.y;
            WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionZ = position.z;





        }

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
        //PlayerCamera.instance.SetLockCameraHeight();
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
        player.playerCombatManager.isFiringBow = true;

        if (player.playerEquipmentManager.notchedArrow != null)
            Destroy(player.playerEquipmentManager.notchedArrow);

        Animator bowAnimator = player.playerInventoryManager.currentRightHandWeapon.weaponModel.GetComponentInChildren<Animator>();

        if (bowAnimator == null)
            return;

        bowAnimator.SetBool("isDrawn", false);
        bowAnimator.Play("Bow_Fire");

        player.canMove = false;

        // Get projectile item
        RangedProjectileItem projectileItem = null;

        switch (currentProjectileBeingUsed)
        {
            case ProjectileSlot.Main:
                projectileItem = player.playerInventoryManager.mainProjectile;
                break;
            case ProjectileSlot.Secondary:
                projectileItem = player.playerInventoryManager.secondaryProjectile;
                break;
        }

        if (projectileItem == null || projectileItem.currentAmmoAmount <= 0)
            return;

        projectileItem.currentAmmoAmount--;

        
        Camera cam = PlayerCamera.instance.cameraObject.GetComponent<Camera>();

        Ray cameraRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        float maxDistance = 50f;
        int mask = LayerMask.GetMask("Default", "Damageable Character");

        if (Physics.Raycast(cameraRay, out hit, maxDistance, mask))
            targetPoint = hit.point;
        else
            targetPoint = cameraRay.GetPoint(maxDistance);

        
        Transform spawnTransform = player.playerCombatManager.LockOnTransform;

        GameObject arrow =Instantiate(projectileItem.releaseProjectileModel, spawnTransform.position, Quaternion.identity);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = arrow.GetComponent<RangedProjectileDamageCollider>();

        if (rb == null || damageCollider == null)
            return;

        
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        damageCollider.characterShootingProjectile = player;

        // Ignore self collision BEFORE force
        Collider[] selfColliders = player.GetComponentsInChildren<Collider>();
        foreach (var col in selfColliders)
        {
            Physics.IgnoreCollision(damageCollider.GetComponent<Collider>(), col, true);
        }

        // =========================
        // 4️⃣ PERFECT AIM DIRECTION
        // =========================
        Vector3 direction =
            (targetPoint - spawnTransform.position).normalized;

        arrow.transform.rotation = Quaternion.LookRotation(direction);

        // Small offset to avoid immediate self-hit
        arrow.transform.position += direction * 0.2f;

        // =========================
        // 5️⃣ FIRE
        // =========================
        rb.AddForce(direction * projectileItem.forwardVelocity, ForceMode.Impulse);

        arrow.transform.parent = null;

        if (!isAimLockedOn)
        {
            StartCoroutine(ExitAimNextFrame());
        }
        




    }


    public void OnBowFireAnimationFinished()
    {
       
        isFiringBow = false;

        if (isAimLockedOn)
        {
            PlayerInputManager.Instance.RB_Input = true;
        }
        
    }



    public void FireBullet()
    {
        if (!player.playerInventoryManager.FireBullet)
            return;

        // GET PROJECTILE DATA
        RangedProjectileItem projectileItem = null;

        switch (currentProjectileBeingUsed)
        {
            case ProjectileSlot.Main:
                projectileItem = player.playerInventoryManager.mainProjectile;
                break;
        }

        if (projectileItem == null || projectileItem.currentMagazineAmmo <= 0)
            return;

        projectileItem.currentMagazineAmmo--;

        AudioClip fireBulletSfx = player.playerInventoryManager.currentRightHandWeapon.fireBulletSound;


        // RayCast from center of screen
        Camera cam = PlayerCamera.instance.cameraObject;

        Ray cameraRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        float maxDistance = 100f;
        int mask = LayerMask.GetMask("Default", "Damageable Character");

        if (Physics.Raycast(cameraRay, out hit, maxDistance, mask))
            targetPoint = hit.point;
        else
            targetPoint = cameraRay.GetPoint(maxDistance);

        
        // SPAWN BULLET
        Transform spawnTransform = player.playerEquipmentManager.projectileInstantiationTransform;

        GameObject bullet = Instantiate(projectileItem.releaseProjectileModel, spawnTransform.position, Quaternion.identity);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        RangedProjectileDamageCollider damageCollider =
            bullet.GetComponent<RangedProjectileDamageCollider>();

        if (rb == null || damageCollider == null)
        {
            Destroy(bullet);
            return;
        }

        
        // PHYSICS SETUP
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        damageCollider.characterShootingProjectile = player;
        damageCollider.physicalDamage = 20;

        // Ignore self collision BEFORE velocity
        Collider[] selfColliders = player.GetComponentsInChildren<Collider>();
        foreach (var col in selfColliders)
        {
            Physics.IgnoreCollision(damageCollider.damageCollider, col, true);
        }


        // Set Direction Before Firing
        Vector3 direction = (targetPoint - spawnTransform.position).normalized;

        bullet.transform.rotation = Quaternion.LookRotation(direction);

        // Prevent immediate self-hit
        bullet.transform.position += direction * 0.1f;

        bullet.transform.parent = null;

        // 6FIRE (DETERMINISTIC)
        rb.linearVelocity = direction * projectileItem.forwardVelocity;


        // AUDIO / VFX
        player.playerSoundFxManager.PlaySoundfx(fireBulletSfx);
    }




    public void ExitBowAimAfterFire()
    {
        player.isAiming = false;
        player.animator.SetBool("isAiming", false);
        PlayerCamera.instance.OnIsAimingChanged(false);
    }

    IEnumerator ExitAimNextFrame()
    {
        yield return null; // wait one frame
        ExitBowAimAfterFire();
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

        //Play Reload SFX
        AudioClip reloadSfx = player.playerInventoryManager.currentRightHandWeapon.reloadGunSound;
        if (reloadSfx != null)
        {
            player.playerSoundFxManager.PlaySoundfx(reloadSfx);
        }
            




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


    public override void OnWeaponRecoil()
    {
        if (!character.isAttacking)
            return;

        // Cancel attack
        character.isAttacking = false;

        // Stop combo
        canComboWithMainHandleWeapon = false;
        canComboWithOffHandleWeapon = false;

        // Play recoil animation
        character.characterAnimatorManager.PlayTargetActionAnimationInstantly("AttackRecoil",true, false);
    }

    public void CheckForWallRecoil(MeleeWeaponItem weapon)
    {
        if (weapon == null)
            return;

        Vector3 origin = character.transform.position + Vector3.up * 1.2f;
        Vector3 direction = character.transform.forward;

        if (Physics.SphereCast(origin, weapon.wallCheckRadius, direction,out RaycastHit hit, weapon.wallCheckDistance, LayerMask.GetMask("Default")))
        {
            OnWeaponRecoil();
        }
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

    public void TriggerBowAimEvent()
    {
        if (player.isHoldingArrow)
            return;

        if (!isAimLockedOn)
            return;

        player.isHoldingArrow = true;
        player.animator.SetBool("isHoldingArrow", true);
        
    }









}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Action/ Ranged Attack Action")]
public class AIRangedAttackAction : AICharacterAttackAction
{
    [Header("Projectile Item")]
    public RangedProjectileItem projectileItem;
    public float fireDelay = 0.25f;
    [SerializeField] Transform projectileInstantiationLocation;

    [Header("Warm Up")]
    public string warmUpAnimation;   // e.g. "Bow_Draw"
    public float warmUpTime = 1f;    // how long to hold before releasing
    public bool isProjectileReadyToLaunch = true;
    public string releaseAnimation;



    public override void AttemptToPerformAction(AICharacterManager aiCharacter)
    {
        projectileInstantiationLocation = aiCharacter.GetComponentInChildren<projectileInstantiationLocation>().transform;

        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Bow_Draw", true, true);
        aiCharacter.animator.SetBool("isProjectileReadyToLaunch", true);

        aiCharacter.StartCoroutine(WarmUpAndFire(aiCharacter));

    }

    private IEnumerator WarmUpAndFire(AICharacterManager aiCharacter)
    {
        float timer = 0f;

        // Hold aim during warm-up
        while (timer < warmUpTime)
        {
            if (aiCharacter.aiCharacterCombatManager.currentTarget)
            {
                aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        // Step 3: Release animation (optional)
        if (isProjectileReadyToLaunch)
        {
            aiCharacter.animator.SetBool("isProjectileReadyToLaunch", false);

        }

        // Step 4: Fire after short delay (sync with anim event or small wait)
        yield return new WaitForSeconds(fireDelay);

        SpawnProjectile(aiCharacter);

    }

    private void SpawnProjectile(AICharacterManager aiCharacter)
    {
        if (projectileItem == null) return;

        // Where to spawn from
        Transform spawn = aiCharacter.aiCharacterCombatManager.LockOnTransform;
        // or use a custom "Muzzle" transform if you have one on the model

        GameObject projectileGameObject = Object.Instantiate(
            projectileItem.releaseProjectileModel,
            spawn.position,
            spawn.rotation
        );

        Rigidbody rb = projectileGameObject.GetComponent<Rigidbody>();
        RangedProjectileDamageCollider damageCollider = projectileGameObject.GetComponent<RangedProjectileDamageCollider>();

        if (damageCollider != null)
        {
            damageCollider.physicalDamage = projectileItem.physicalDamage;
            damageCollider.characterShootingProjectile = aiCharacter;
        }

        // Aim at current target
        if (aiCharacter.aiCharacterCombatManager.currentTarget != null)
        {
            Vector3 targetPos = aiCharacter.aiCharacterCombatManager.currentTarget.characterCombatManager.LockOnTransform.position;
            projectileGameObject.transform.LookAt(targetPos);
        }

        // Ignore AI’s own colliders
        Collider[] selfColliders = aiCharacter.GetComponentsInChildren<Collider>();
        foreach (var col in selfColliders)
            Physics.IgnoreCollision(damageCollider.damageCollider, col, true);

        // Apply force
        rb.AddForce(projectileGameObject.transform.forward * projectileItem.forwardVelocity);
    }



}

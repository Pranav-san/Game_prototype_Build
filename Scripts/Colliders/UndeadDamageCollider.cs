using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadDamageCollider : DamageCollider
{

    AICharacterManager aiCharacterManager;

    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        aiCharacterManager = GetComponentInParent<AICharacterManager>();
        damageCollider.enabled = false;


    }
    protected override void DamageTarget(CharacterManager damageTarget)
    {




        if (damageTarget == aiCharacterManager)
        {
            return;
        }

        if (charactersDamaged.Contains(damageTarget))
        {
            return;
        }
        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.firelDamage = firelDamage;
        damageEffect.lightininglDamage = lightininglDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(aiCharacterManager.transform.forward, damageTarget.transform.forward, Vector3.up);

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

    }

    

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualDamageCollider : DamageCollider
{

    AICharacterManager characterCausingDamage;


    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        characterCausingDamage = GetComponentInParent<AICharacterManager>();
        damageCollider.enabled = false;


    }

    protected override void GetBlockingDotValues(CharacterManager damageTarget)
    {
        directionFromAttackToDamageTarget =  characterCausingDamage.transform.position - damageTarget.transform.position;
        dotvalueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
    }
    protected override void DamageTarget(CharacterManager damageTarget)
    {




        if (damageTarget == characterCausingDamage)
        {
            return;
        }
        if (damageTarget.characterGroup == characterCausingDamage.characterGroup)
            return;

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
        damageEffect.poiseDamage = poiseDamage;
        damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

    }

    

    

}

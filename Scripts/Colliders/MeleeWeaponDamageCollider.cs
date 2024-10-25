using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider

    
{

    [Header("Attacking character")]
    public CharacterManager characterCausingDamage;// This is used to check Attackers Damage modifers,effects etc

    [Header("Weapon Attack Modifier")]
    public float light_Attack_01_Modifier;

    protected override void Awake()
    {
        base.Awake();
        damageCollider = GetComponent<Collider>();
        damageCollider.enabled = false;

    }

    protected override void  OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

        if (damageTarget != null)
        { 
            if (damageTarget == null || damageTarget==characterCausingDamage)
                return;

            contactPoint = other.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        }
        DamageTarget(damageTarget);

    }

    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if (damageTarget == null || charactersDamaged.Contains(damageTarget))
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
        damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        switch(characterCausingDamage.characterCombatManager.currentAttackType)
        {
            case AttackType.LightAttack01:
                ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);

                break;
        }

        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

    }

    private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
    {

        damage.physicalDamage *= modifier;
        damage.magicDamage *= modifier;
        damage.firelDamage *= modifier;
        damage.lightininglDamage *= modifier;

    }



}



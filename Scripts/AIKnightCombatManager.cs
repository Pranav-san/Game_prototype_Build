using UnityEngine;

public class AIKnightCombatManager : AICharacterCombatManager
{

    [Header("Damage Colliders")]
    [SerializeField] ManualDamageCollider swordDamageCollider;
    //[SerializeField]ManualDamageCollider leftDamageCollider;

    [Header("Damage Modifiers")]
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;

    public void SetAttack01Damage()
    {

        
        swordDamageCollider.physicalDamage = baseDamage*attack01DamageModifier;
        //leftDamageCollider.physicalDamage = baseDamage*attack01DamageModifier*attack02DamageModifier;

        swordDamageCollider.poiseDamage = basePoiseDamage*attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        swordDamageCollider.physicalDamage = baseDamage*attack02DamageModifier;
        //leftDamageCollider.physicalDamage = baseDamage*attack02DamageModifier*attack02DamageModifier;

        swordDamageCollider.poiseDamage = basePoiseDamage*attack02DamageModifier;
    }

    public void OpenSwordDamageCollider()
    {
        swordDamageCollider.EnableDamageCollider();
    }

    public void CloseSwordDamageCollider()
    {
        swordDamageCollider.DisableDamageCollider();
    }

}

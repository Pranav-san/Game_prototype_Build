using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItems : Item
{

    [Header("Spell Modifiers")]
    public float fullChargeEffectMultiplier = 2;

    public int spellSlotUsed = 1;

    [Header("spell FX")]
    [SerializeField] protected GameObject spellCastWarmUpFX;
    [SerializeField] protected GameObject spellCastReleaseFX;

    //Full Charge Version

    [Header("AnimationState")]
    [SerializeField] protected string mainHandSpellAnimation;
    [SerializeField] protected string offHandSpellAnimation;

    //Play War UP Animation Befor Casting Spell
    public virtual void AttemptToCastSpell(playerManager player)
    {

    }

    //Play Throw or Cast Spell Animation
    public virtual void SuccessfullyCastSpell(playerManager player)
    {

    }

    protected virtual void InstantiateWarmUpSpellFX(playerManager player)
    {

    }

    protected virtual void InstantiateWarmUpReleaseFX(playerManager player)
    {

    }

    public virtual bool CanIUseThisItem()
    {
        return true;

    }







}

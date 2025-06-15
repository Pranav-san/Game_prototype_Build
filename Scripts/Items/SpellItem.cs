using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{

    [Header("Spell Class")]
    public SpellClass spellClass;

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

    [Header("Sound FX")]
    public AudioClip WarmUpSoundFx;
    public AudioClip releaseSoundFX;
    

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

    protected virtual void InstantiateReleaseFX(playerManager player)
    {

    }

    public virtual bool CanICastThisSpell(playerManager player)
    {
        return true;

    }







}

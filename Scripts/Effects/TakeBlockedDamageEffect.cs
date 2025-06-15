using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/ Instant Effects/ Take Blocked Damage ")]
public class TakeBlockedDamageEffect : InstantCharacterEffect
{

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float firelDamage = 0;
    public float lightininglDamage = 0;

    [Header("Final Damage")]
    private int finalDamageDealt = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")]
    public bool willPlayDamageSfx = true;
    public AudioClip elementalSounfFX;

    [Header("Direction Damage Taken from")]
    public float angleHitFrom;
    public Vector3 contactPoint;
    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        Debug.Log("Hit was blocked");

        if (character.isDead)
        {
            return;
        }

        CalculateDamage(character.characterStatsManager);
        if (!character.characterStatsManager.isDead)
        {
            PlayDirectionalBasedBlockingAnimation(character);

        }

        playDamageSfX(character);
        playBlockDamageVFX(character);


    }
    private void CalculateDamage(CharacterStatsManager character)
    {
        Debug.Log("original Damage " + physicalDamage );

        physicalDamage -=(physicalDamage*(character.blockingPhysicalAbsorption/100));
        firelDamage -=(firelDamage*(character.blockingFireAbsorption/100));
        lightininglDamage -=(lightininglDamage*(character.blockingLightningAbsorption/100));
        magicDamage -=(magicDamage*(character.blockingMagicAbsorption/100));


        finalDamageDealt= Mathf.RoundToInt(physicalDamage + magicDamage + firelDamage + lightininglDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt =1;
        }
        Debug.Log("Blocked Final Damage" + physicalDamage);
        character.ConsumeHealth(finalDamageDealt);




    }

    private void PlayDirectionalBasedBlockingAnimation(CharacterManager character)
    {
        if (character.isDead)
            return;

        DamageIntensity damageIntensity = WorldUtilityManager.Instance.GetDamageIntensityBasedOnPoiseDamage(poiseDamage);

        switch (damageIntensity)
        {
            case DamageIntensity.Ping:
                damageAnimation = "Block Reaction 1";
                break;
            case DamageIntensity.Light:
                damageAnimation = "Block Reaction 2";
                break;
            case DamageIntensity.Medium:
                damageAnimation = "Block Reaction 1";
                break;
            case DamageIntensity.Heavy:
                damageAnimation = "Block_Heavy_01";
                break;
            case DamageIntensity.Collasal:
                damageAnimation = "Block_Collasl_01";
                break;
            default:
                break;

        }
        character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation,true);


    }


    private void playDamageSfX(CharacterManager character)
    {
        character.characterSoundFxManager.playBlockSoundfx();
    }

    private void playBlockDamageVFX(CharacterManager character)
    {

        character.characterEffectsManager.PlayBlockedVFX(contactPoint);

    }

}

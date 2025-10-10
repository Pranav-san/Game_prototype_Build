using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/ Instant Effects/ Take Damage ")]
public class TakeDamageEffect : InstantCharacterEffect
{

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float firelDamage = 0;
    public float lightininglDamage = 0;

    [Header("Final Damage")]
    protected int finalDamageDealt = 0;

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

        if (character.characterStatsManager.isDead)
            return;

        if (character.isInvulnerable)
            return;

       
        
        CalculateDamage(character.characterStatsManager);
        if(!character.characterStatsManager.isDead)
        {
            PlayDirectionalBasedDamageAnimation(character);
            character.characterEffectsManager.InterruptEffect();


        }

        if(PlayerInputManager.Instance.player.hasArrowNotched)
        {
            PlayerInputManager.Instance.player.hasArrowNotched=false;
            Destroy(PlayerInputManager.Instance.player.playerEquipmentManager.notchedArrow);
            PlayerInputManager.Instance.player.playerEquipmentManager.notchedArrow = null;

        }
        
        PlayeDamageVFX(character);
        PlayDamageSFX(character);

        //Run This After All Other Functions Attempt To play Animation
        CalculateStanceDamage(character);




    }
    protected virtual void CalculateDamage(CharacterStatsManager character)
    {


        finalDamageDealt= Mathf.RoundToInt(physicalDamage + magicDamage + firelDamage + lightininglDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt =1;
        }
        //Debug.Log("FinalDamage: "+ finalDamageDealt); 
        character.ConsumeHealth(finalDamageDealt);

        //character.lastAttacker = PlayerInputManager.Instance.character;

        //We Subtract Poise Damage From Characters Total
        character.totalPoiseDamage -= poiseDamage;

        float remainingPoise = character.basePoiseDefense + character.offensivePoiseBonus+ character.totalPoiseDamage;

        if (remainingPoise <= 0) 
            poiseIsBroken = true;

        character.poiseResetTimer = character.defaultPoiseResetTimer;

       




    }

    protected void CalculateStanceDamage(CharacterManager character)
    {
        AICharacterManager aiCharacter = character as AICharacterManager;


        //Optionally Give Weapons Their Ow Stance Damage, Or use Poise Damage
         int stanceDamage = Mathf.RoundToInt(poiseDamage);

        if (aiCharacter != null)
        {
            aiCharacter.aiCharacterCombatManager.DamageStance(stanceDamage);
        }






    }

    protected void PlayeDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);

    }

    protected void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
        AudioClip weaponDamageSFX = WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(WorldSoundFXManager.instance.weaponDamageSFX);
        
        character.characterSoundFxManager.PlaySoundfx(physicalDamageSFX);

       
        

        



    }

    protected void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {

        if(character.characterStatsManager.isDead)
            return;

        if (poiseIsBroken)
        {
            if (angleHitFrom >=145 && angleHitFrom <=180)
            {
                //Play Forward Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.forward_Medium_Damage);

            }
            else if (angleHitFrom <=-145 && angleHitFrom >=-180)
            {
                //Play Forward Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.forward_Medium_Damage);


            }
            else if (angleHitFrom >=-45 && angleHitFrom <=45)
            {
                //Play Backward Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.backward_Medium_Damage);


            }
            else if (angleHitFrom >=-144 && angleHitFrom <=-45)
            {
                //Play Left Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.left_Medium_Damage);


            }
            else if (angleHitFrom >=45 && angleHitFrom <=144)
            {
                //Play Right Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.right_Medium_Damage);


            }

        }
        else
        {
            if (angleHitFrom >=145 && angleHitFrom <=180)
            {
                //Play Forward Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.forward_Ping_Damage);

            }
            else if (angleHitFrom <=-145 && angleHitFrom >=-180)
            {
                //Play Forward Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.forward_Ping_Damage);


            }
            else if (angleHitFrom >=-45 && angleHitFrom <=45)
            {
                //Play Backward Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.backward_Ping_Damage);


            }
            else if (angleHitFrom >=-144 && angleHitFrom <=-45)
            {
                //Play Left Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.left_Ping_Damage);


            }
            else if (angleHitFrom >=45 && angleHitFrom <=144)
            {
                //Play Right Damage Animation
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationsFromList(character.characterAnimatorManager.right_Ping_Damage);


            }
        }

        character.characterAnimatorManager.lastAnimationPlayed = damageAnimation;


        //If Poise is broken play staggering animation
        if (poiseIsBroken)
        {
            //If Poise Broken Restrict Movement and Actions
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            

        }
        else
        {
            //If Poise is not Broken Play Flinch Animation (UPPER BODY),  Allow Movement, Rotation, Actions
            character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, false, false, true, true,true, true);
        }


    }

}

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
    private int finalDamageDealt = 0;

    [Header("Poise")]
    public float poise = 0;
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

        if(character.isDead)
        {
            return;
        }
        CalculateDamage(character.characterStatsManager);
        if(!character.characterStatsManager.isDead)
        {
            PlayDirectionalBasedDamageAnimation(character);

        }
        

    }
    private void CalculateDamage(CharacterStatsManager character)
    {


        finalDamageDealt= Mathf.RoundToInt(physicalDamage + magicDamage + firelDamage + lightininglDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt =1;
        }
        Debug.Log("FinalDamage: "+ finalDamageDealt); 
        character.ConsumeHealth(finalDamageDealt);


    }

    private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if(angleHitFrom >=145 && angleHitFrom <=180)
        {
            //Play Forward Damage Animation
            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterAnimatorManager.hit_Forward_Medium_01,true);

        }
        else if (angleHitFrom <=-145 && angleHitFrom >=-180)
        {
            //Play Forward Damage Animation
            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterAnimatorManager.hit_Forward_Medium_01, true);


        }
        else if (angleHitFrom >=-45 && angleHitFrom <=45)
        {
            //Play Backward Damage Animation
            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterAnimatorManager.hit_Backward_Medium_01, true);


        }
        else if (angleHitFrom >=-144 && angleHitFrom <=-45)
        {
            //Play Left Damage Animation
            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterAnimatorManager.hit_Left_Medium_01, true);


        }
        else if (angleHitFrom >=45 && angleHitFrom <=144)
        {
            //Play Right Damage Animation
            character.characterAnimatorManager.PlayTargetActionAnimation(character.characterAnimatorManager.hit_Right_Medium_01, true);


        }


    }

}
